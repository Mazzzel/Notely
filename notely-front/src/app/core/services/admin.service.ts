import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CompteAdminDTO, UpdateAccesPagesDTO } from '../models/admin.model';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Admin`;

  getComptes() {
    return this.http.get<CompteAdminDTO[]>(`${this.base}/GetComptes`);
  }

  setPages(idCompte: number, dto: UpdateAccesPagesDTO) {
    return this.http.put<void>(`${this.base}/SetPages/${idCompte}`, dto);
  }
}
