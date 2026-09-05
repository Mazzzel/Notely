import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CompteDTO, LoginResponseDTO } from '../models/compte.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Auth`;

  private compte = signal<CompteDTO | null>(null);

  readonly currentUser = this.compte.asReadonly();
  readonly isLoggedIn = computed(() => this.compte() !== null);
  readonly mustChangePassword = computed(() => this.compte()?.doitChangerMotDePasse ?? false);

  login(email: string, motDePasse: string): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${this.base}/Login`, { email, motDePasse }).pipe(
      tap((res) =>
        this.compte.set({
          idCompte: res.idCompte,
          email: res.email,
          doitChangerMotDePasse: res.doitChangerMotDePasse,
          dateCreation: '',
          dateDerniereConnexion: null
        })
      )
    );
  }

  me(): Observable<CompteDTO | null> {
    return this.http.get<CompteDTO>(`${this.base}/Me`).pipe(
      tap((res) => this.compte.set(res)),
      catchError(() => {
        this.compte.set(null);
        return of(null);
      })
    );
  }

  changePassword(motDePasseActuel: string, nouveauMotDePasse: string): Observable<void> {
    return this.http.post<void>(`${this.base}/ChangePassword`, { motDePasseActuel, nouveauMotDePasse }).pipe(
      tap(() => {
        const current = this.compte();
        if (current) this.compte.set({ ...current, doitChangerMotDePasse: false });
      })
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.base}/Logout`, {}).pipe(
      tap(() => this.compte.set(null)),
      catchError(() => {
        this.compte.set(null);
        return of(void 0);
      })
    );
  }
}
