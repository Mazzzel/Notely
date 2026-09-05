import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { NoteCreateDTO, NoteDTO, NoteUpdateDTO } from '../models/note.model';

@Injectable({ providedIn: 'root' })
export class NoteService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Note`;

  getAll() {
    return this.http.get<NoteDTO[]>(`${this.base}/GetAll`);
  }

  create(dto: NoteCreateDTO) {
    return this.http.post<NoteDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: NoteUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
