import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CoursCreateDTO, CoursDTO, CoursDetailDTO, CoursUpdateDTO } from '../models/cours.model';

@Injectable({ providedIn: 'root' })
export class CoursService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Cours`;

  getAll() {
    return this.http.get<CoursDTO[]>(`${this.base}/GetAll`);
  }

  getById(id: number) {
    return this.http.get<CoursDetailDTO>(`${this.base}/GetById/${id}`);
  }

  create(dto: CoursCreateDTO) {
    return this.http.post<CoursDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: CoursUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
