import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { TodoService } from './services/todo.service';
import { TodoFormComponent } from './components/todo-form/todo-form';
import { TodoListComponent } from './components/todo-list/todo-list';
import type { TodoEntry } from './models/todo.model';

@Component({
  selector: 'app-root',
  imports: [TodoFormComponent, TodoListComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  @ViewChild(TodoFormComponent) private todoForm!: TodoFormComponent;
  @ViewChild(TodoListComponent) private todoList!: TodoListComponent;

  private readonly todoService = inject(TodoService);

  protected readonly todos = signal<TodoEntry[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadTodos();
  }

  private getReadableError(err: unknown, fallback: string): string {
    const payload =
      (err && typeof err === 'object' && 'error' in err ? (err as { error?: unknown }).error : undefined) ?? err;

    if (typeof payload === 'string' && payload.trim()) {
      return payload;
    }

    if (payload && typeof payload === 'object') {
      const record = payload as Record<string, unknown>;
      const problemDetails =
        record['problemDetails'] && typeof record['problemDetails'] === 'object'
          ? (record['problemDetails'] as Record<string, unknown>)
          : null;
      const nestedError =
        record['error'] && typeof record['error'] === 'object'
          ? (record['error'] as Record<string, unknown>)
          : null;

      const details = problemDetails ?? nestedError ?? record;

      const candidates = [
        record['message'],
        problemDetails?.['message'],
        nestedError?.['message'],
        details['message'],
        details['detail'],
        details['title'],
      ];

      for (const candidate of candidates) {
        if (typeof candidate === 'string' && candidate.trim()) {
          return candidate;
        }
      }

      const errors = Array.isArray(details['errors'])
        ? details['errors']
        : Array.isArray(problemDetails?.['errors'])
          ? problemDetails['errors']
          : Array.isArray(nestedError?.['errors'])
            ? nestedError['errors']
            : [];

      for (const item of errors) {
        if (typeof item === 'string' && item.trim()) {
          return item;
        }

        if (item && typeof item === 'object' && 'message' in item) {
          const message = (item as { message?: string }).message;
          if (typeof message === 'string' && message.trim()) {
            return message;
          }
        }
      }
    }

    if (typeof (err as { message?: string })?.message === 'string' && (err as { message?: string }).message!.trim()) {
      return (err as { message?: string }).message!;
    }

    return fallback;
  }

  private loadTodos(): void {
    this.loading.set(true);
    this.error.set(null);
    this.todoService.getAll().subscribe({
      next: (entries) => {
        this.todos.set(entries);
        this.loading.set(false);
      },
      error: (err: unknown) => {
        this.error.set(this.getReadableError(err, 'Failed to load todos.'));
        this.loading.set(false);
      },
    });
  }

  protected onTodoAdded(todo: string): void {
    this.todoService.create(todo).subscribe({
      next: () => {
        this.todoForm?.reset();
        this.loadTodos();
      },
      error: (err: unknown) => {
        this.todoForm?.setError(this.getReadableError(err, 'Failed to add todo.'));
      },
    });
  }

  protected onTodoUpdated(event: { id: string; todo: string }): void {
    this.todoService.update(event.id, event.todo).subscribe({
      next: () => {
        this.todoList?.clearBusy();
        this.loadTodos();
      },
      error: (err: unknown) => {
        this.error.set(this.getReadableError(err, 'Failed to update todo.'));
        this.todoList?.clearBusy();
      },
    });
  }

  protected onTodoDeleted(id: string): void {
    this.todoService.remove(id).subscribe({
      next: () => {
        this.todoList?.clearBusy();
        this.loadTodos();
      },
      error: (err: unknown) => {
        this.error.set(this.getReadableError(err, 'Failed to delete todo.'));
        this.todoList?.clearBusy();
      },
    });
  }
}
