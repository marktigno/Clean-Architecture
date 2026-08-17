import { useState } from "react";
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

  if (todos.length === 0) {
    return <p className="todo-empty">No todos yet. Add one above!</p>;
  }

  const startEdit = (todo: TodoEntry) => {
    setEditingId(todo.id);
    setEditValue(todo.todo.value);
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditValue("");
  };

  const saveEdit = async (id: string) => {
    const trimmed = editValue.trim();
    if (!trimmed) {
      return;
    }
    setBusyId(id);
    try {
      await onUpdate(id, trimmed);
      setEditingId(null);
    } finally {
      setBusyId(null);
    }
  };

  const handleDelete = async (id: string) => {
    setBusyId(id);
    try {
      await onDelete(id);
    } finally {
      setBusyId(null);
    }
  };

  return (
    <ul className="todo-list">
      {todos.map((entry) => (
        <li key={entry.id} className="todo-item">
          {editingId === entry.id ? (
            <>
              <input
                type="text"
                value={editValue}
                onChange={(event) => setEditValue(event.target.value)}
                disabled={busyId === entry.id}
              />
              <button onClick={() => saveEdit(entry.id)} disabled={busyId === entry.id}>
                Save
              </button>
              <button onClick={cancelEdit} disabled={busyId === entry.id}>
                Cancel
              </button>
            </>
          ) : (
            <>
              <span className="todo-text">{entry.todo.value}</span>
              <button onClick={() => startEdit(entry)} disabled={busyId === entry.id}>
                Edit
              </button>
              <button onClick={() => handleDelete(entry.id)} disabled={busyId === entry.id}>
                Delete
              </button>
            </>
          )}
        </li>
      ))}
    </ul>
  );
}
