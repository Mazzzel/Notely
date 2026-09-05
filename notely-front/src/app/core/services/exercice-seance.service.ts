import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ExerciceSeanceCreateDTO, ExerciceSeanceDTO } from '../models/seance.model';

@Injectable({ providedIn: 'root' })
export class ExerciceSeanceService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/ExerciceSeance`;

  create(dto: ExerciceSeanceCreateDTO) {
    return this.http.post<ExerciceSeanceDTO>(`${this.base}/Post`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
