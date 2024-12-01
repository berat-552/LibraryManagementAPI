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
To set up the database, you will need to use Entity Framework Core migrations. Here are the steps:

#### Add Initial Migration
This command creates a migration script for the initial database schema.
```bash
dotnet ef migrations add initialCreate
```

#### List All Migrations

```bash
dotnet ef migrations list
```
#### Revert to the previous migration
```bash
dotnet ef database update PreviousMigrationName
```

#### Remove the latest migration
```bash
dotnet ef migrations remove
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
To run the tests for the project, use the following command inside the project directory:

```sh
dotnet test
```
Alternatively, use the test button in Visual Studio 2022 to run all tests.

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

### Library Member
| Field       | Type     | Description                        |
|-------------|----------|------------------------------------|
| `Id`        | `int`    | Unique identifier for the library member   |
| `Username`      | `string` | The username of the library member     |
| `Email` | `string` | The email address of the library member      |
| `Password` | `string` | The password for the library member      |

## API Documentation
The API is documented using Swagger, providing a UI to explore all endpoints and try them out.

- Swagger UI: Available at https://localhost:7147/swagger

### JWT Authentication
This API uses JWT (JSON Web Token) for authentication. To access protected endpoints, you need to include a valid JWT token in the Authorization header of your requests. You can obtain a token by logging in with valid credentials.

To use JWT in Swagger:

Click on the Authorize button in the Swagger UI.
Enter your JWT token in the Value box.
Once authorized, you can make authenticated requests to the protected endpoints.

![image](https://github.com/user-attachments/assets/37954b7d-5c9a-4ee9-8a85-8528cde90271)

## Endpoints

#### Get all Authors (Authorized)
```bash
GET /api/v1/authors
```

#### Create a new Author (Authorized)
```bash
POST /api/v1/authors
```

#### Get an Author by ID (Authorized)
```bash
GET /api/v1/authors/{id}
```

#### Update an Author (Authorized)
```bash
PUT /api/v1/authors/{id}
```

#### Delete an Author (Authorized)
```bash
DELETE /api/v1/authors/{id}
```
---

#### Get all Books (Authorized)
```bash
GET /api/v1/books
```

#### Create a new book (Authorized)
```bash
POST /api/v1/books
```

#### Get a Book by ID (Authorized)
```bash
GET /api/v1/books/{id}
```

#### Update a Book (Authorized)
```bash
PUT /api/v1/books{id}
```

#### Delete an Author (Authorized)
```bash
DELETE /api/v1/authors/{id}
```
---

#### Get a Library Member by ID (Authorized)
```bash
GET /api/v1/librarymembers/{id}
```

#### Register a New Library Member
```bash
POST /api/v1/librarymembers/register
```
#### Login as a Library Member
```bash
POST /api/v1/librarymembers/login
```

#### Update a Library Member (Authorized)
```bash
PUT /api/v1/librarymembers/{id}
```

#### Delete a Library Member (Authorized)
```bash
DELETE /api/v1/librarymembers/{id}
```
