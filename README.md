# LibraryManagementAPI

A RESTful ASP.NET Core Web API for managing books, authors, and loan records in a library.

## Features

- Manage books, authors, and loans.

## Installation

Clone the repository and run using Visual Studio 2022

```bash
# Clone the repository
git clone https://github.com/berat-552/LibraryManagementAPI.git
```

## Models

### Book
| Field           | Type       | Description                    |
|-----------------|------------|--------------------------------|
| `Id`            | `int`      | Unique identifier for the book |
| `Title`         | `string`   | Title of the book              |
| `ISBN`          | `string`   | International Standard Book Number|
| `Genre`         | `string`   | Genre of the book              |
| `AuthorId`      | `int`      | ID of the author               |
| `PublishedDate` | `DateTime` | Date the book was published    |
| `IsAvailable`   | `bool`     | Availability status            |

### Author
| Field       | Type     | Description                        |
|-------------|----------|------------------------------------|
| `Id`        | `int`    | Unique identifier for the author   |
| `Name`      | `string` | Name of the author                 |
| `Biography` | `string` | Short biography of the author      |

### Loan
| Field        | Type        | Description                           |
|--------------|-------------|---------------------------------------|
| `Id`         | `int`       | Unique identifier for the loan        |
| `BookId`     | `int`       | ID of the book being loaned           |
| `LoanDate`   | `DateTime`  | Date when the book was loaned         |
| `ReturnDate` | `DateTime?` | Date when the book was returned, if returned |

## API Documentation
The API is documented using Swagger, providing a UI to explore all endpoints and try them out.

- Swagger UI: Available at https://localhost:7147/swagger
