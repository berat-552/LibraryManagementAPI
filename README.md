# LibraryManagementAPI

A RESTful ASP.NET Core Web API for managing books and authors in a library.

## Features

- Manage books and authors.

## Installation

Clone the repository and run using Visual Studio 2022

```bash
# Clone the repository
git clone https://github.com/berat-552/LibraryManagementAPI.git
```

## Database Setup (SQLite)
To set up the database, you will need to use Entiy Framework Core migrations. Here are the steps:

#### Add Initial Migration
This command creates a migration script for the initial database schema.
```bash
dotnet ef migrations ef add initialCreate
```

#### Apply Migrations
This command applies all pending migrations to the database, creating or updating the database schema.
```bash
dotnet ef database update
```

#### Update Database with New Migrations
Whenever you make changes to your models, create a new migration and update the database.
```bash
# Create a new migration
dotnet ef migrations add <MigrationName>

# Apply the new migration
dotnet ef database update
```

#### Remove the Most Recent Migration
If you need to remove the most recent migration that has not yet been applied to the database, use the following command:
```bash
dotnet ef migrations remove
```
This command will delete the most recent migration files and update the model snapshot to reflect the state before the migration.

#### 

## Running Tests
To run the tests for the project, use the following command:

```sh
dotnet test
```
Alternatively, use the test button in Visual Studio 2022 to run all tests

## Models

### Book
| Field           | Type       | Description                    |
|-----------------|------------|--------------------------------|
| `Id`            | `int`      | Unique identifier for the book |
| `BookTitle`         | `string`   | Title of the book              |
| `ISBN`          | `string`   | International Standard Book Number|
| `Genre`         | `string`   | Genre of the book              |
| `AuthorId`      | `int`      | ID of the author               |
| `PublishedDate` | `DateTime` | Date the book was published    |

### Author
| Field       | Type     | Description                        |
|-------------|----------|------------------------------------|
| `Id`        | `int`    | Unique identifier for the author   |
| `AuthorName`      | `string` | Name of the author                 |
| `Biography` | `string` | Short biography of the author      |

## API Documentation
The API is documented using Swagger, providing a UI to explore all endpoints and try them out.

- Swagger UI: Available at https://localhost:7147/swagger

## Endpoints

#### Get all Authors
```bash
GET /api/v1/authors
```

#### Create a new Author
```bash
POST /api/v1/authors
```

#### Get an Author by ID
```bash
GET /api/v1/authors/{id}
```

#### Update an Author
```bash
PUT /api/v1/authors/{id}
```

#### Delete an Author
```bash
DELETE /api/v1/authors/{id}
```
