# Hotel Back Office Backend

This project is a **hotel back-office management system backend** built with **ASP.NET Core**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.  

It allows managing **bookings, customers, rooms, and invoices**. The backend is designed to be modular, testable, and maintainable.  

The repository includes **Docker configuration** to run a SQL Server database for local development.

## Features
* Create, update, and cancel bookings
* Manage customers and rooms
* Generate invoices for bookings
* Clean Architecture and SOLID principles
* Application Service pattern with Result pattern for business logic (example)
  
## Project Structure
* Domain/ -> Aggregates, Value Objects, Entities
* Application/ -> Services, DTOs, Interfaces
* Infrastructure/ -> Repositories, Database context, EF Core Setup
* Presentation/ -> ASP.NET Core Web API controllers

---

## Quick Start

Follow these steps to get the project running locally:

```Bash
git clone https://github.com/mikkotirronen/hotel-backoffice.git
cd hotel-backoffice
docker-compose up -d
dotnet ef database update --project Infrastructure --startup-project Presetation.Api
cd Presentation
dotnet run
```

### 1. Clone the repository

```Bash
git clone https://github.com/your-username/hotel-backoffice.git
cd hotel-backoffice
```

### 2. Start the SQL Server database with Docker

The project includes a docker-compose.yml file to run SQL Server in a container. To start the database:

```Bash
docker-compose up -d 
```
* This starts SQL Server 2022 in a container named ```hotel-db```
* Database credentials (for demo purposes only)
  * Username: sa
  * Password: Super@86secret
* SQL Server will be available on ```localhost:1433```
* Data is persisted in the Docker volume ```sql_data```

  You can verify the container is running:
```Bash
docker ps
```

### 3. Update the backend connection string (optional)

  Ensure your backend uses the correct SQL Server connection string. Open appsettings.Development.json and confirm:

```JSON
{
  "ConnectionStrings": {
    "HotelDb": "Server=localhost,1433;Database=HotelDb;User Id=sa;Password=Super@86secret;TrustServerCertificate=True;"
  }
}
```

### 4. Apply migrations

Run the following commands from the solution root:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation.Api
```

This will create the database and apply all migrations.

---

### 5. Run the application

```bash
dotnet run --project Presentation.Api
```

---

### 6. Run the backend API

Navigate to presentation project and run the API:
```Bash
  cd Presentation
  dotnet run
```

### 7. Open Swagger

Navigate to:

```
https://localhost:5041/swagger
```
or
```
https://localhost:<port>/swagger
```

### 8. Stop the project

```Bash
docker-compose down
```
* The sql-data Docker volume preserves your database between restarts.

### 9. Reset Database

```Bash
dotnet ef database drop --project Infrastructure --startup-project Presentation.Api
dotnet ef database update --project Infrastructure --startup-project Presentation.Api
```

## Project Structure
* Domain/ -> Aggregates, Value Objects, Entities
* Application/ -> Services, DTOs, Interfaces
* Infrastructure/ -> Repositories, Database context, EF Core Setup
* Presentation/ -> ASP.NET Core Web API controllers

## Features
* Create, update, and cancel bookings
* Manage customers and rooms
* Generate invoices for bookings
* Clean Architecture and SOLID principles
* Application Service pattern with Result pattern for business logic (example)


