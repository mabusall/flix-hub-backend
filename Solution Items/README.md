# FlixHub REST API (ASP.NET Core + PostgreSQL + EF Core)

This project is a **RESTful API** built with **.NET Core** and **Entity Framework Core** using **PostgreSQL** as the database.  
It is designed to integrate with the **FlixHub Electron + Vite** app.

---

## 🚀 Features

- ASP.NET Core 8.0 REST API
- PostgreSQL database
- Entity Framework Core (Code-First + Migrations)
- Repository & Service pattern
- Swagger (OpenAPI) for API documentation
- Cross-Origin Resource Sharing (CORS) enabled for Electron frontend
- Docker support (optional)

---

## 🛠️ Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio Code](https://code.visualstudio.com/) or [Rider](https://www.jetbrains.com/rider/)
- (Optional) [Docker](https://www.docker.com/)

---

## 📂 Project Structure

FlixHub.Api/
├── Controllers/ # API Controllers
├── Data/ # DbContext & Migrations
├── Models/ # Entity Models
├── Repositories/ # Repository Layer
├── Services/ # Business Logic Layer
├── Program.cs # Entry Point
└── appsettings.json # Configuration
