import type {
  ApiResult,
  GetTodoEntriesResponse,
  GetTodoEntryByIdResponse,
  ProblemDetailsResponse,
  TodoEntry,
} from "../types/todo";

const DEV_API_BASE_URL = "/api/TodoEntries";
const HTTPS_API_BASE_URL = "https://localhost:5001/api/TodoEntries";
const HTTP_API_BASE_URL = "http://localhost:5000/api/TodoEntries";
const JSON_CONTENT_TYPE_MARKERS = ["application/json", "application/problem+json"];

function getApiBaseUrls(): string[] {
  const configuredBaseUrl = import.meta.env.VITE_API_BASE_URL?.trim();

  if (configuredBaseUrl) {
    return [configuredBaseUrl];
  }

  if (import.meta.env.DEV) {
    return [DEV_API_BASE_URL];
  }

  return [HTTPS_API_BASE_URL, HTTP_API_BASE_URL];
}

function getPreferredApiBaseUrl(): string {
  return getApiBaseUrls()[0];
}

function isJsonResponse(response: Response): boolean {
  const contentType = response.headers.get("content-type");

  return (
    contentType !== null &&
    JSON_CONTENT_TYPE_MARKERS.some((marker) => contentType.toLowerCase().includes(marker))
  );
}

function getApiConfigurationHint(): string {
  return `Check that the WebApi is running and VITE_API_BASE_URL points to the todo endpoint (current: ${getPreferredApiBaseUrl()}).`;
}

function createUnexpectedResponseMessage(response: Response, body: string): string {
  const preview = body.trim().slice(0, 80);

  if (preview.startsWith("<")) {
    return `Expected a JSON response from the todo API but received HTML (${response.status}). ${getApiConfigurationHint()}`;
  }

  return `Expected a JSON response from the todo API but received ${response.headers.get("content-type") ?? "unknown content"} (${response.status}). ${getApiConfigurationHint()}`;
}

async function parseJsonBody<T>(response: Response): Promise<T> {
  const body = await response.text();

  if (!isJsonResponse(response)) {
    throw new Error(createUnexpectedResponseMessage(response, body));
  }

  try {
    return JSON.parse(body) as T;
  } catch (error) {
    if (error instanceof SyntaxError) {
      throw new Error(
        `The todo API returned invalid JSON (${response.status}). ${getApiConfigurationHint()}`,
      );
    }

    throw error;
  }
}

function createNetworkErrorMessage(triedUrls: string[]): string {
  return `Unable to reach the todo API. Check that the WebApi is running and that the frontend API/proxy configuration is correct. Tried: ${triedUrls.join(", ")}`;
}

async function fetchTodoApi(path = "", init?: RequestInit): Promise<Response> {
  const triedUrls: string[] = [];
  let lastError: unknown;

  for (const baseUrl of getApiBaseUrls()) {
    const requestUrl = `${baseUrl}${path}`;
    triedUrls.push(requestUrl);

    try {
      return await fetch(requestUrl, init);
    } catch (error) {
      lastError = error;

      if (!(error instanceof TypeError)) {
        throw error;
      }
    }
  }

  if (lastError instanceof TypeError) {
    throw new Error(createNetworkErrorMessage(triedUrls));
  }

  throw lastError;
}

async function handleResponse<T>(response: Response): Promise<T> {
  if (response.status === 204) {
    return undefined as T;
  }

  if (!response.ok) {
    if (isJsonResponse(response)) {
      const problem = await parseJsonBody<{ problemDetails?: ProblemDetailsResponse }>(response);
      const message =
        problem.problemDetails?.errors?.[0]?.message ??
        problem.problemDetails?.detail ??
        problem.problemDetails?.title ??
        `Request failed with status ${response.status}`;

      throw new Error(message);
    }

    const body = await response.text();
    throw new Error(createUnexpectedResponseMessage(response, body));
  }

  return await parseJsonBody<T>(response);
}

export async function getAllTodoEntries(): Promise<TodoEntry[]> {
  const response = await fetchTodoApi();
  const data = await handleResponse<ApiResult<GetTodoEntriesResponse>>(response);
  return data.value.todoEntries;
}

export async function getTodoEntryById(id: string): Promise<TodoEntry> {
  const response = await fetchTodoApi(`/${id}`);
  const data = await handleResponse<ApiResult<GetTodoEntryByIdResponse>>(response);
  return data.value.todoEntry;
}

export async function createTodoEntry(todo: string): Promise<void> {
  const response = await fetchTodoApi("/create", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ todo }),
  });
  await handleResponse<void>(response);
}

export async function updateTodoEntry(id: string, todo: string): Promise<void> {
  const response = await fetchTodoApi("/update", {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ id, todo }),
  });
  await handleResponse<void>(response);
}

export async function deleteTodoEntry(id: string): Promise<void> {
  const response = await fetchTodoApi("/delete", {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ id }),
  });
  await handleResponse<void>(response);
}
