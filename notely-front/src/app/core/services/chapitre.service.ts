import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ChapitreCreateDTO, ChapitreDTO, ChapitreUpdateDTO } from '../models/chapitre.model';

@Injectable({ providedIn: 'root' })
export class ChapitreService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Chapitre`;

  getByCours(idCours: number) {
    return this.http.get<ChapitreDTO[]>(`${this.base}/GetByCours/${idCours}`);
  }

  create(dto: ChapitreCreateDTO) {
    return this.http.post<ChapitreDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: ChapitreUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
