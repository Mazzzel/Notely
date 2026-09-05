import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CompteDTO, LoginResponseDTO } from '../models/compte.model';

const TOKEN_STORAGE_KEY = 'notely-token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Auth`;

  private compte = signal<CompteDTO | null>(null);
  private token = signal<string | null>(this.readStoredToken());

  readonly currentUser = this.compte.asReadonly();
  readonly isLoggedIn = computed(() => this.compte() !== null);
  readonly mustChangePassword = computed(() => this.compte()?.doitChangerMotDePasse ?? false);

  getToken(): string | null {
    return this.token();
  }

  login(email: string, motDePasse: string): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${this.base}/Login`, { email, motDePasse }).pipe(
      tap((res) => {
        this.setToken(res.token);
        this.compte.set({
          idCompte: res.idCompte,
          email: res.email,
          doitChangerMotDePasse: res.doitChangerMotDePasse,
          dateCreation: '',
          dateDerniereConnexion: null
        });
      })
    );
  }

  me(): Observable<CompteDTO | null> {
    return this.http.get<CompteDTO>(`${this.base}/Me`).pipe(
      tap((res) => this.compte.set(res)),
      catchError(() => {
        this.clearAuth();
        return of(null);
      })
    );
  }

  changePassword(motDePasseActuel: string, nouveauMotDePasse: string): Observable<void> {
    return this.http.post<{ token: string }>(`${this.base}/ChangePassword`, { motDePasseActuel, nouveauMotDePasse }).pipe(
      tap((res) => {
        this.setToken(res.token);
        const current = this.compte();
        if (current) this.compte.set({ ...current, doitChangerMotDePasse: false });
      }),
      map(() => void 0)
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.base}/Logout`, {}).pipe(
      tap(() => this.clearAuth()),
      catchError(() => {
        this.clearAuth();
        return of(void 0);
      })
    );
  }

  private setToken(token: string): void {
    this.token.set(token);
    try {
      localStorage.setItem(TOKEN_STORAGE_KEY, token);
    } catch {
      /* stockage indisponible, on continue avec le token en mémoire */
    }
  }

  private clearAuth(): void {
    this.compte.set(null);
    this.token.set(null);
    try {
      localStorage.removeItem(TOKEN_STORAGE_KEY);
    } catch {
      /* stockage indisponible, rien à nettoyer */
    }
  }

  private readStoredToken(): string | null {
    try {
      return localStorage.getItem(TOKEN_STORAGE_KEY);
    } catch {
      return null;
    }
  }
}
