import { Component, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-todo-form',
  imports: [FormsModule],
  templateUrl: './todo-form.html',
  styleUrl: './todo-form.css',
})
export class TodoFormComponent {
  readonly added = output<string>();

  protected readonly value = signal('');
  protected readonly submitting = signal(false);
  protected readonly error = signal<string | null>(null);

  protected onSubmit(): void {
    const trimmed = this.value().trim();
    if (!trimmed) {
      this.error.set('Todo cannot be empty.');
      return;
    }
    this.submitting.set(true);
    this.error.set(null);
    this.added.emit(trimmed);
  }

  reset(): void {
    this.value.set('');
    this.submitting.set(false);
    this.error.set(null);
  }

  setError(message: string): void {
    this.error.set(message);
    this.submitting.set(false);
  }
}
