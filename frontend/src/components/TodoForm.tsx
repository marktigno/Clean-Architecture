import { useState } from "react";

interface TodoFormProps {
  onAdd: (todo: string) => Promise<void>;
}

const MAX_LENGTH = 100;

export function TodoForm({ onAdd }: TodoFormProps) {
  const [value, setValue] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const trimmed = value.trim();

    if (!trimmed) {
      setError("Todo cannot be empty.");
      return;
    }

    setSubmitting(true);
    setError(null);

    try {
      await onAdd(trimmed);
      setValue("");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to add todo.");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="todo-form" onSubmit={handleSubmit}>
      <input
        type="text"
        value={value}
        maxLength={MAX_LENGTH}
        placeholder="What needs to be done?"
        onChange={(event) => setValue(event.target.value)}
        disabled={submitting}
      />
      <button type="submit" disabled={submitting}>
        Add
      </button>
      {error && <p className="todo-error">{error}</p>}
    </form>
  );
}
