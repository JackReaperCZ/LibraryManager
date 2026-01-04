# Library Management System

A desktop application for library management developed in C# (Windows Forms) with MySQL database.

## Features

- **Manage Books, Authors, Members**: CRUD operations.
- **Loan Management**: Issue and return books with transaction support.
- **Reports**: Generate aggregated reports (Member Loan Counts, Popular Books, Genre Stats).
- **Import**: Import Books and Members from CSV/JSON files.
- **Configuration**: `appsettings.json` for database connection and settings.
- **Repository Pattern**: Custom implementation for data access.

## Prerequisites

- .NET 8.0 SDK or later
- MySQL Server

## Setup

1. **Database Setup**:
   - Create a MySQL database (e.g., `LibraryDB`).
   - Run the script `doc/schema.sql` to create tables and views.

2. **Configuration**:
   - Edit `config/appsettings.json`.
   - Update `ConnectionString` with your MySQL credentials.
   - Update `ImportExportPath` to a valid directory for import/export operations.

3. **Build and Run**:
   - Open the solution `LibraryManager.sln` in Visual Studio or Trae IDE.
   - Build the project.
   - Run the application.

## Project Structure

- `src/Forms`: Windows Forms UI.
- `src/Models`: Data models (Book, Member, etc.).
- `src/Repositories`: Data access logic (Repository Pattern).
- `src/Services`: Business logic services (Import).
- `config`: Configuration files.
- `doc`: Documentation and SQL scripts.
- `test`: Test data.

## Usage

- **Books**: View, Add, Edit, Delete, Import.
- **Authors**: View, Add, Edit, Delete.
- **Members**: View, Add, Edit, Delete, Import.
- **Loans**: View active loans, Issue new loan (checks availability), Return book.
- **Reports**: View statistics.

## License

This project is for educational purposes.
