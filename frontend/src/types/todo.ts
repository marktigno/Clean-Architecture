export interface Todo {
  value: string;
}

export interface TodoEntry {
  id: string;
  todo: Todo;
  createdDateTime: string | null;
  modifiedDateTime: string | null;
}

export interface GetTodoEntriesResponse {
  todoEntries: TodoEntry[];
}

export interface GetTodoEntryByIdResponse {
  todoEntry: TodoEntry;
}

/** The Result envelope the backend wraps every response in. */
export interface ApiResult<T> {
  value: T;
  isSuccess: boolean;
  isFailure: boolean;
  error: {
    code: string;
    message: string;
  };
}
