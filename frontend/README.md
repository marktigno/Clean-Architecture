# Clean Architecture — Angular Frontend

An Angular (standalone components) client for the To-Do list WebAPI in [`src/WebApi`](../src/WebApi).

## Prerequisites

- Node.js 18+ and npm
- Angular CLI 17+ (`npm install -g @angular/cli`)
- The WebAPI running locally (see [`src/WebApi`](../src/WebApi)), by default at `http://localhost:5000`

## Setup

```powershell
cd frontend-angular
npm install
```

The API base URL is configured in `src/environments/environment.development.ts`:

```ts
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000/api/TodoEntries',
};
```

## Run

```powershell
ng serve
```

The app runs at `http://localhost:4200` by default. Make sure the WebApi is running (`dotnet run --project ../src/WebApi`) so requests succeed. The WebApi's CORS policy (`AllowedOrigins` in `Program.cs`) allows `http://localhost:4200`.

## Build

```powershell
ng build
```

## Features

- List, create, update, and delete todo entries via the `api/TodoEntries` MVC controller endpoints.
- Simple form to add new todos and inline editing/deleting of existing ones.

## Project structure

- `src/environments/` — environment configuration (API base URL).
- `src/app/models/todo.model.ts` — TypeScript interfaces matching the backend DTOs.
- `src/app/services/todo.service.ts` — Angular service using `HttpClient` to call the API.
- `src/app/components/todo-form/` — standalone component for adding a new todo.
- `src/app/components/todo-list/` — standalone component for listing, editing, and deleting todos.
- `src/app/app.ts` — root component wiring service + components with loading/error state.
