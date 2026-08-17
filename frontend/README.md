# Clean Architecture — Frontend

A React + TypeScript (Vite) client for the To-Do list WebAPI in [`src/WebApi`](../src/WebApi).

## Prerequisites

- Node.js 18+ and npm
- The WebAPI running locally (see [`src/WebApi`](../src/WebApi)), by default at `http://localhost:5000`

## Setup

```powershell
cd frontend
npm install
```

Configure the API base URL in `.env` (already set to match the WebApi `http` launch profile):

```
VITE_API_BASE_URL=http://localhost:5000/api/TodoEntries
```

## Run

```powershell
npm run dev
```

The app runs at `http://localhost:5173` by default. Make sure the WebApi is running (`dotnet run --project ../src/WebApi`) so requests succeed. The WebApi's CORS policy (`AllowedOrigins` in `Program.cs`) already allows `http://localhost:5173`.

## Build

```powershell
npm run build
```

## Features

- List, create, update, and delete todo entries via the `api/TodoEntries` MVC controller endpoints.
- Simple form to add new todos and inline editing/deleting of existing ones.

## Project structure

- `src/api/todoApi.ts` — typed fetch client for the TodoEntries API.
- `src/types/todo.ts` — TypeScript types matching the backend DTOs.
- `src/components/TodoForm.tsx` — form to add a new todo.
- `src/components/TodoList.tsx` — list with inline edit/delete.
- `src/App.tsx` — wires everything together with loading/error states.
