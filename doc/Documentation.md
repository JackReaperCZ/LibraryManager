# Library Management System - Documentation

## Database Schema

The database consists of the following tables:

1. **Books**: Stores book details (Title, Price, Genre, etc.).
2. **Authors**: Stores author details.
3. **Members**: Stores library member details.
4. **Loans**: Tracks book loans (Who borrowed what and when).
5. **BookAuthors**: M:N relationship between Books and Authors.

Views:
- **CurrentLoans**: Lists currently borrowed books.
- **OverdueLoans**: Lists books that are overdue (if return date logic is applied).

## Repository Pattern Implementation

The application uses a custom Repository pattern to abstract data access.

- **IRepository<T>**: Generic interface defining CRUD operations.
- **BaseRepository**: Abstract base class handling database connections using `MySqlConnection`.
- **Concrete Repositories**:
  - `BookRepository`: Handles `Books` table.
  - `MemberRepository`: Handles `Members` table.
  - `AuthorRepository`: Handles `Authors` table.
  - `LoanRepository`: Handles `Loans` table and transactions.
  - `ReportRepository`: Handles complex queries for reports.

## Architecture

- **UI Layer**: Windows Forms (Forms folder). Handles user interaction and input validation.
- **Service Layer**: `ImportService` for handling file imports.
- **Data Access Layer**: Repositories (Repositories folder). Directly interacts with MySQL.
- **Configuration**: `AppConfig` class loads settings from `appsettings.json`.

## Error Handling

- **Database Errors**: Caught in Repositories/UI, displayed to user.
- **Validation**: Input fields are validated in Forms (e.g., required fields).
- **Import Errors**: File format or duplicate key errors are handled during import.

## How to Run without IDE

1. Publish the application:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
2. Navigate to `./publish` directory.
3. Ensure `appsettings.json` is configured correctly.
4. Run `LibraryManager.exe`.
