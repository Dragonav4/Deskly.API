# Hoteling System - Backend
This repository contains the server-side implementation of the Hoteling System.
## Technologies
- **C# / .NET 8** - Core platform.
- **ASP.NET Core Web API** - RESTful services.
- **Entity Framework Core** - ORM for database operations.
- **SQLite** - Database (local file `hoteling.db`).
- **Google OAuth 2.0 / OpenID Connect** - Authentication.
- **Swagger / OpenAPI** - API documentation and testing.
- **DotNetEnv** - Environment variable management via `.env`.
## Installation and Configuration
### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
### Clone and Dependencies
1. Navigate to the project directory:
   ```bash
   cd Hoteling.API
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
### Environment Variables
The project uses a `.env` file for secrets. Create a `.env` file in the root of `Hoteling.API` (based on `.env.template`):
```env
GOOGLE_CLIENT_ID=your_client_id
GOOGLE_CLIENT_SECRET=your_client_secret
```
### Database
The project is configured to use SQLite. Database migrations are automatically applied during application startup via the `db.Database.Migrate()` call in `Program.cs`.
To create a new migration:
```bash
dotnet ef migrations add MigrationName --project ../Hoteling.Infrastructure --startup-project .
```
To apply migrations manually:
```bash
dotnet ef database update --project ../Hoteling.Infrastructure --startup-project .
```
## Running the Project
Start the project from the API root directory:
```bash
dotnet run
```
After startup:
- API will be available at: `http://localhost:5126`
- Swagger UI (Documentation): `http://localhost:5126/swagger` (available in Development mode)
## Roles and Permissions

The system implements role-based access control (RBAC) with the following roles:

### Guest (Unauthenticated or Role=Guest)
- **Desks**: View list of desks and basic desk information.
- **Reservations**: View if a desk is occupied and on which dates.
- **Privacy**: Cannot see identity of the person who made the reservation.
- **Restricted**: Cannot create, edit, or delete any data.

### Employee
- **View Access**: Full access to see all reservation details, including who reserved the desk.
- **Reservations**: Create reservations for themselves.
- **Management**: Edit or delete their own reservations.

### Admin
- **Full Access**: All Employee permissions.
- **Desks Management**: Full CRUD access to the Desks entity (Create, Update, Delete desks).
- **System Management**: Ability to manage system-wide settings and entities.

## Project Structure
- `Hoteling.API` - Controllers, middleware, and configuration.
- `Hoteling.Application` - Business logic, services, and mappers.
- `Hoteling.Domain` - Entities and data models.
- `Hoteling.Infrastructure` - Data access (Repositories, DbContext) and external integrations.
## Authentication
Accessing protected endpoints (`[Authorize]`) requires Google authentication. The frontend is pre-configured to handle the authentication flow with this API.
