import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import type {
  ApiResult,
  GetTodoEntriesResponse,
  GetTodoEntryByIdResponse,
  TodoEntry,
} from '../models/todo.model';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiBaseUrl;

  getAll(): Observable<TodoEntry[]> {
    return this.http
      .get<ApiResult<GetTodoEntriesResponse>>(this.apiUrl)
      .pipe(map((result) => result.value.todoEntries));
  }

  getById(id: string): Observable<TodoEntry> {
    return this.http
      .get<ApiResult<GetTodoEntryByIdResponse>>(`${this.apiUrl}/${id}`)
      .pipe(map((result) => result.value.todoEntry));
  }

  create(todo: string): Observable<void> {
    return this.http
      .post<void>(`${this.apiUrl}/create`, { todo });
  }

  update(id: string, todo: string): Observable<void> {
    return this.http
      .put<void>(`${this.apiUrl}/update`, { id, todo });
  }

  remove(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete`, {
      body: { id },
    });
  }
}
