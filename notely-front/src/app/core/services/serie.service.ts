import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { SerieCreateDTO, SerieDTO, SerieUpdateDTO } from '../models/seance.model';

@Injectable({ providedIn: 'root' })
export class SerieService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Serie`;

  create(dto: SerieCreateDTO) {
    return this.http.post<SerieDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: SerieUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
