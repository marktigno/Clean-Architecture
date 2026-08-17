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

  private loadTodos(): void {
    this.loading.set(true);
    this.error.set(null);
    this.todoService.getAll().subscribe({
      next: (entries) => {
        this.todos.set(entries);
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.error.set(err.message ?? 'Failed to load todos.');
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
      error: (err: Error) => {
        this.todoForm?.setError(err.message ?? 'Failed to add todo.');
      },
    });
  }

  protected onTodoUpdated(event: { id: string; todo: string }): void {
    this.todoService.update(event.id, event.todo).subscribe({
      next: () => {
        this.todoList?.clearBusy();
        this.loadTodos();
      },
      error: (err: Error) => {
        this.error.set(err.message ?? 'Failed to update todo.');
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
      error: (err: Error) => {
        this.error.set(err.message ?? 'Failed to delete todo.');
        this.todoList?.clearBusy();
      },
    });
  }
}
