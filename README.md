# LibraryAPI

Library management REST API built with ASP.NET Core and Entity Framework Core.

## Requirements

- .NET SDK 10.0
- SQL Server
- A database configured in `appsettings.json`

## Run locally

From the project directory:

```powershell
dotnet restore
dotnet build
dotnet run
```

Default URLs:

- HTTP: `http://localhost:5099`
- HTTPS: `https://localhost:7078`

Swagger is available in Development at `/swagger`.

## Configuration

The application reads its settings from `appsettings.json`.

Configure the SQL Server connection and JWT settings before running:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "a-development-key-at-least-32-bytes-long",
    "Issuer": "LibraryApiIssuer",
    "Audience": "LibraryApiAudience",
    "ExpiryMinutes": 60
  }
}
```

Do not commit production passwords or JWT keys. Rotate any credentials that have been exposed and use a secret-management solution for production deployments.

## Authentication

The current demo login accepts credentials as query parameters:

```http
POST /api/Auth/login?username=admin&password=password
```

A successful login returns a JWT and writes an `access_token` HTTP-only cookie. Protected requests can use the cookie or a bearer header:

```http
Authorization: Bearer {token}
```

All library management endpoints require an authenticated token with the `Admin` role.

## Endpoints

### Authentication

| Method | Endpoint | Description |
| --- | --- | --- |
| `POST` | `/api/Auth/login?username={username}&password={password}` | Sign in |

### Libraries

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/Libraries` | List libraries with books and members |
| `GET` | `/api/Libraries/{id}` | Get one library with books and members |
| `POST` | `/api/Libraries` | Create a library |
| `PUT` | `/api/Libraries/{id}` | Update a library |
| `DELETE` | `/api/Libraries/{id}` | Delete a library |

Create or update body:

```json
{
  "name": "Central Library",
  "address": "1 Main Street"
}
```

### Books

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/Books` | List books with related library data |
| `GET` | `/api/Books/{id}` | Get one book |
| `POST` | `/api/Books` | Add a book |
| `PUT` | `/api/Books/{id}` | Update a book |
| `DELETE` | `/api/Books/{id}` | Delete a book |

Create or update body:

```json
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "9780132350884",
  "publishedYear": 2008,
  "libraryId": 1
}
```

### Members

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/Members` | List members with related library data |
| `GET` | `/api/Members/{id}` | Get one member |
| `POST` | `/api/Members` | Add a member |
| `PUT` | `/api/Members/{id}` | Update a member |
| `DELETE` | `/api/Members/{id}` | Delete a member |

Create or update body:

```json
{
  "name": "Jane Doe",
  "email": "jane@example.com",
  "libraryId": 1
}
```

### Loans

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/Loans/member/{memberId}` | List a member's loans |
| `POST` | `/api/Loans?bookId={bookId}&memberId={memberId}` | Borrow a book |
| `PUT` | `/api/Loans/{loanId}/return` | Return a book |

Borrowing validates that the book and member exist, belong to the same library, and that the book is not already on loan.

## Database migrations

Apply existing migrations with:

```powershell
dotnet ef database update
```

Create a migration after model changes with:

```powershell
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Tests

Run all tests:

```powershell
dotnet test
```

Tests use the EF Core InMemory provider where appropriate.
