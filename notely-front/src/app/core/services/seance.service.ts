import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ProgressionPointDTO, SeanceCreateDTO, SeanceDTO, SeanceUpdateDTO } from '../models/seance.model';

@Injectable({ providedIn: 'root' })
export class SeanceService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Seance`;

  getAll() {
    return this.http.get<SeanceDTO[]>(`${this.base}/GetAll`);
  }

  getById(id: number) {
    return this.http.get<SeanceDTO>(`${this.base}/GetById/${id}`);
  }

  getExercices() {
    return this.http.get<string[]>(`${this.base}/GetExercices`);
  }

  getProgression(exercice: string) {
    return this.http.get<ProgressionPointDTO[]>(`${this.base}/GetProgression`, { params: { exercice } });
  }

  create(dto: SeanceCreateDTO) {
    return this.http.post<SeanceDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: SeanceUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
