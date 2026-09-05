import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { TodoCreateDTO, TodoDTO, TodoUpdateDTO } from '../models/todo.model';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/Todo`;

  getAll() {
    return this.http.get<TodoDTO[]>(`${this.base}/GetAll`);
  }

  getByCours(idCours: number) {
    return this.http.get<TodoDTO[]>(`${this.base}/GetByCours/${idCours}`);
  }

  create(dto: TodoCreateDTO) {
    return this.http.post<TodoDTO>(`${this.base}/Post`, dto);
  }

  update(id: number, dto: TodoUpdateDTO) {
    return this.http.put<void>(`${this.base}/Put/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Delete/${id}`);
  }
}
