# Library Management System
 This project is a Library Management System built with ASP.NET Core Web API and Entity Framework Core (EF Core 8), using SQL Server as the database. The system allows management of library books, authors, and users, with CRUD operations and default seeded data for initial setup.

 ## Key Features ✨
 * Admin -> 
    Admin can do the below functionalities
    * Book Management: Add, update, delete, and retrieve books.
    * User Management: Add, update, delete, and retrieve users.
    * Author Management: Store and manage author details.
* Users -> From User prespective 
    * Borrow book
    * Return book
    * Search book

## Technologies Used
* ASP.NET Core 8.0 (Web API)
* Entity Framework Core 8
* SQL Server for database
* Dependency Injection for services
* Swagger for API documentation

## Getting Started 🧑‍💻
Follow these steps to set up the project locally.
### Prerequisites
* .NET SDK 8.0+ installed
* SQL Server (LocalDB or a SQL Server instance)
* Visual Studio 2022 or VS Code with C# extensions


## Run Locally 🏠  
1) ### Clone the project  

~~~bash  
  git clone https://github.com/Elayaraja1609/LibraryManagementSystem.git
~~~

2) ### Configure the Database Connection:
* Update the appsettings.json file with your SQL Server connection string:
~~~bash
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;"
}
~~~
3) ### Run Migrations:
 Generate and apply migrations to set up the database schema and seed data:
~~~bash
dotnet ef migrations add InitialCreate
dotnet ef database update
~~~
4) ### Run the Project:
~~~bash
dotnet run
~~~
5) ### Access Swagger for API Documentation:
* Navigate to `http://localhost:5273/swagger` to view and test the API endpoints.

## Default Seeded Data
The application starts with some default categories, such as Fiction, Non-Fiction, Science, History, Biography, and more. This data is automatically seeded in the Category table upon migration.

## Usage
* Once the application is running, open your browser and go to:

    * GET /v1/admin/books: Retrieve a list of all books.
    * POST /v1/admin/books: Add a new book.
    * PUT /v1/admin/books/{id}: Update an existing book.
    * DELETE /v1/admin/books/{id}: Delete a book by ID.
* This will display the full Swagger UI with detailed API documentation, including all available endpoints, request and response structures, and sample data.

## Contributing
Contributions are welcome! Feel free to submit a pull request or open an issue to discuss improvements or report bugs.
