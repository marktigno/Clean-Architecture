import type {
  ApiResult,
  GetTodoEntriesResponse,
  GetTodoEntryByIdResponse,
  TodoEntry,
} from "../types/todo";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    let message = `Request failed with status ${response.status}`;
    try {
      const problem = await response.json();
      message = problem.detail ?? problem.title ?? message;
    } catch {
      // response had no JSON body; keep default message
    }
    throw new Error(message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

export async function getAllTodoEntries(): Promise<TodoEntry[]> {
  const response = await fetch(API_BASE_URL);
  const data = await handleResponse<ApiResult<GetTodoEntriesResponse>>(response);
  return data.value.todoEntries;
}

export async function getTodoEntryById(id: string): Promise<TodoEntry> {
  const response = await fetch(`${API_BASE_URL}/${id}`);
  const data = await handleResponse<ApiResult<GetTodoEntryByIdResponse>>(response);
  return data.value.todoEntry;
}

export async function createTodoEntry(todo: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/create`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ todo }),
  });
  await handleResponse<void>(response);
}

export async function updateTodoEntry(id: string, todo: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/update`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ id, todo }),
  });
  await handleResponse<void>(response);
}

export async function deleteTodoEntry(id: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/delete`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ id }),
  });
  await handleResponse<void>(response);
}
