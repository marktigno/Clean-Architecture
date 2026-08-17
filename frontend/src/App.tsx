import { useCallback, useEffect, useState } from "react";
import "./App.css";
import { TodoForm } from "./components/TodoForm";
import { TodoList } from "./components/TodoList";
import {
  createTodoEntry,
  deleteTodoEntry,
  getAllTodoEntries,
  updateTodoEntry,
} from "./api/todoApi";
import type { TodoEntry } from "./types/todo";

function App() {
  const [todos, setTodos] = useState<TodoEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadTodos = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const entries = await getAllTodoEntries();
      setTodos(entries);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to load todos.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadTodos();
  }, [loadTodos]);

  const handleAdd = async (todo: string) => {
    await createTodoEntry(todo);
    await loadTodos();
  };

  const handleUpdate = async (id: string, todo: string) => {
    await updateTodoEntry(id, todo);
    await loadTodos();
  };

  const handleDelete = async (id: string) => {
    await deleteTodoEntry(id);
    await loadTodos();
  };

  return (
    <main className="todo-app">
      <h1>To-Do List</h1>
      <TodoForm onAdd={handleAdd} />
      {loading && <p>Loading todos...</p>}
      {error && <p className="todo-error">{error}</p>}
      {!loading && !error && (
        <TodoList todos={todos} onUpdate={handleUpdate} onDelete={handleDelete} />
      )}
    </main>
  );
}

export default App;
