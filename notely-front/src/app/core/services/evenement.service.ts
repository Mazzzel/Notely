import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { EvenementCreateDTO, EvenementDTO, EvenementUpdateDTO } from '../models/evenement.model';

@Injectable({ providedIn: 'root' })
export class EvenementService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Evenement`;

  getAll() {
    return this.http.get<EvenementDTO[]>(`${this.base}/GetAll`);
  }

  create(dto: EvenementCreateDTO) {
    return this.http.post<EvenementDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: EvenementUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
