import { Component, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import type { TodoEntry } from '../../models/todo.model';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog';

@Component({
  selector: 'app-todo-list',
  imports: [FormsModule, ConfirmDialogComponent],
  templateUrl: './todo-list.html',
  styleUrl: './todo-list.css',
})
export class TodoListComponent {
  readonly todos = input<TodoEntry[]>([]);
  readonly updated = output<{ id: string; todo: string }>();
  readonly deleted = output<string>();

  protected readonly editingId = signal<string | null>(null);
  protected readonly editValue = signal('');
  protected readonly busyId = signal<string | null>(null);
  protected readonly pendingDeleteId = signal<string | null>(null);

  protected startEdit(entry: TodoEntry): void {
    this.editingId.set(entry.id);
    this.editValue.set(entry.todo.value);
  }

  protected cancelEdit(): void {
    this.editingId.set(null);
    this.editValue.set('');
  }

  protected saveEdit(id: string): void {
    const trimmed = this.editValue().trim();
    if (!trimmed) return;
    this.busyId.set(id);
    this.updated.emit({ id, todo: trimmed });
  }

  protected requestDelete(id: string): void {
    if (this.busyId()) return;
    this.pendingDeleteId.set(id);
  }

  protected cancelDelete(): void {
    if (this.busyId()) return;
    this.pendingDeleteId.set(null);
  }

  protected confirmDelete(): void {
    const id = this.pendingDeleteId();
    if (!id) return;
    this.busyId.set(id);
    this.pendingDeleteId.set(null);
    this.deleted.emit(id);
  }

  clearBusy(): void {
    this.busyId.set(null);
    this.editingId.set(null);
    this.pendingDeleteId.set(null);
  }
}
