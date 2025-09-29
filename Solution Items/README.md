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

# Database Design Strategy

## 1. Unified Content Table
- Holds **both Movies + TV Series**.  
- `type` column to distinguish (`movie` or `tv`).  
- Normalized `release_date` column (maps to `release_date` for movies, `first_air_date` for TV).  
- Stores high-level metadata:
  - title
  - overview
  - language
  - country
  - runtime
  - status  

---

## 2. Supporting Tables (Shared)
- **genres** → master list of genres.  
- **content_genres** → mapping table (many-to-many).  
- **people** → actors, directors, writers.  
- **content_cast** → mapping table (`content_id + person_id + role`).  
- **content_crew** → mapping table (director, writer, producer, etc.).  
- **images** → posters, backdrops, logos (with type + size).  
- **videos** → trailers, teasers (YouTube keys, type, official).  
- **external_ids** → IMDb, Trakt, etc.  
- **ratings** → IMDb, TMDb, Trakt scores.  

---

## 3. TV-Specific Tables
- **seasons** → belongs to a TV show.  
- **episodes** → belongs to a season.  
- **episode_cast** / **episode_crew** (optional, if you want detailed credits per episode).  

---