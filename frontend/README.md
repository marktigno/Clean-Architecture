# Clean Architecture — Frontend

A React + TypeScript (Vite) client for the To-Do list WebAPI in [`src/WebApi`](../src/WebApi).

## Prerequisites

- Node.js 18+ and npm
- The WebAPI running locally (see [`src/WebApi`](../src/WebApi)), by default at `https://localhost:5001` for the HTTPS launch profile or `http://localhost:5000` for the HTTP launch profile

## Setup

```powershell
cd frontend
npm install
```

Configure the frontend `.env` for local development:

```
VITE_API_BASE_URL=/api/TodoEntries
VITE_API_PROXY_TARGET=https://localhost:5001
```

In development, the frontend calls `/api/TodoEntries` and Vite proxies those requests to `VITE_API_PROXY_TARGET`. This avoids browser certificate/CORS issues with the local HTTPS backend and still lets the UI receive the backend's human-readable JSON error messages. If `.env` is not present, the dev proxy defaults to `https://localhost:5001`.

## Run

```powershell
npm run dev
```

The app runs at `http://localhost:5173` by default. Make sure the WebApi is running (`dotnet run --launch-profile https --project ../src/WebApi` or `dotnet run --launch-profile http --project ../src/WebApi`) so requests succeed. The WebApi's CORS policy (`AllowedOrigins` in `Program.cs`) already allows `http://localhost:5173`.

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
