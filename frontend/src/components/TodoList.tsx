import { useState } from "react";
import { ConfirmDialog } from "./ConfirmDialog";
import type { TodoEntry } from "../types/todo";

interface TodoListProps {
  todos: TodoEntry[];
  onUpdate: (id: string, todo: string) => Promise<void>;
  onDelete: (id: string) => Promise<void>;
}

export function TodoList({ todos, onUpdate, onDelete }: TodoListProps) {
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editValue, setEditValue] = useState("");
  const [busyId, setBusyId] = useState<string | null>(null);
  const [editError, setEditError] = useState<string | null>(null);
  const [todoPendingDelete, setTodoPendingDelete] = useState<TodoEntry | null>(null);

  if (todos.length === 0) {
    return <p className="todo-empty">No todos yet. Add one above!</p>;
  }

  const startEdit = (todo: TodoEntry) => {
    setEditingId(todo.id);
    setEditValue(todo.todo.value);
    setEditError(null);
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditValue("");
    setEditError(null);
  };

  const saveEdit = async (id: string) => {
    const trimmed = editValue.trim();

    setBusyId(id);
    setEditError(null);
    try {
      await onUpdate(id, trimmed);
      setEditingId(null);
      setEditValue("");
    } catch (err) {
      setEditError(err instanceof Error ? err.message : "Failed to update todo.");
    } finally {
      setBusyId(null);
    }
  };

  const requestDelete = (todo: TodoEntry) => {
    setTodoPendingDelete(todo);
  };

  const cancelDelete = () => {
    if (busyId === todoPendingDelete?.id) {
      return;
    }

    setTodoPendingDelete(null);
  };

  const confirmDelete = async () => {
    if (todoPendingDelete === null) {
      return;
    }

    const { id } = todoPendingDelete;
    setBusyId(id);
    try {
      await onDelete(id);
      setTodoPendingDelete(null);
    } finally {
      setBusyId(null);
    }
  };

  return (
    <ul className="todo-list">
      {todos.map((entry) => (
        <li key={entry.id} className="todo-item">
          <div className="todo-item-content">
            {editingId === entry.id ? (
              <>
                <div className="todo-edit-controls">
                  <input
                    type="text"
                    value={editValue}
                    onChange={(event) => {
                      setEditValue(event.target.value);
                      setEditError(null);
                    }}
                    disabled={busyId === entry.id}
                  />
                  <button onClick={() => saveEdit(entry.id)} disabled={busyId === entry.id}>
                    Save
                  </button>
                  <button onClick={cancelEdit} disabled={busyId === entry.id}>
                    Cancel
                  </button>
                </div>
                {editError && <p className="todo-error">{editError}</p>}
              </>
            ) : (
              <div className="todo-item-actions">
                <span className="todo-text">{entry.todo.value}</span>
                <button onClick={() => startEdit(entry)} disabled={busyId === entry.id}>
                  Edit
                </button>
                <button onClick={() => requestDelete(entry)} disabled={busyId === entry.id}>
                  Delete
                </button>
              </div>
            )}
          </div>
        </li>
      ))}
      {todoPendingDelete && (
        <ConfirmDialog
          title="Delete to-do entry?"
          message={`Are you sure you want to delete "${todoPendingDelete.todo.value}"?`}
          confirmLabel="Delete"
          cancelLabel="Cancel"
          busy={busyId === todoPendingDelete.id}
          onConfirm={confirmDelete}
          onCancel={cancelDelete}
        />
      )}
    </ul>
  );
}
