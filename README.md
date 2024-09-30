# C# Betting Application

## Overview
This is a betting application built using **ASP.NET Core 8**. It follows a modular structure, with RabbitMQ for messaging and SQL Server as the database. The application allows players to place wagers and provides analytics on player activity.

## Requirements
- **.NET Core 8 SDK**
- **SQL Server**
- **RabbitMQ**

## Setup Instructions

### Database
1. Navigate to the `Scripts/DatabaseGenerate.sql` file.
2. Run the script on your SQL Server instance to generate the required tables and stored procedures.

### Running the Application
1. Clone the repository to your local machine.
2. Open the solution in your favourite C# IDE (e.g., Visual Studio or Rider).
3. Update the connection strings in `appsettings.json` for both the SQL Server database and RabbitMQ.
4. Build the solution.
5. Run the application. The Swagger UI will be available at `/swagger` for API testing.

### Architectural Overview
The project follows the layered architecture with the following structure:

- **Controllers**: Handles HTTP requests and invokes services to fulfil them.
- **Services**: Business logic and communication with external systems RabbitMQ.
- **Repositories**: Responsible for database interaction using SQL and Dapper.
- **Middleware**: Global exception handling to manage errors consistently.
- **Models**: Contains request and response models.

### Folder Structure

- `Constants/`
  - Contains constant values used across the app.
  - Example: `Constant.cs`
  
- `Controllers/`
  - Handles HTTP requests and maps them to service calls.
  - Example: `CasinoWagerController.cs`
  
- `Interfaces/`
  - Interfaces for services and repositories, ensuring a clear separation of concerns.
  - Example: `ICasinoService.cs`, `IRabbitMqService.cs`

- `Middleware/`
  - Custom middleware for handling global exceptions and errors.
  - Example: `ExceptionMiddleware.cs`

- `Models/`
  - Contains models for requests and responses.
  - Example:
    - `Requests/` - Includes request models like `CasinoRequest.cs`, `PlayerWagerRequest.cs`.
    - `Responses/` - Includes response models like `CasinoWagerResponse.cs`, `ErrorResponse.cs`.

- `Repositories/`
  - Data access layer, responsible for querying the database.
  - Example: `CasinoRepository.cs`

- `Scripts/`
  - SQL scripts for setting up the database.
  - Example: `DatabaseGenerate.sql`

- `Services/`
  - Business logic and interaction with RabbitMQ and repositories.
  - Example: `CasinoService.cs`, `RabbitMqService.cs`

- `Program.cs`
  - Entry point for configuring and running the application.

## Key Features

1. **Player Wager Management**: Players can place wagers, and their data is stored in the database.
2. **RabbitMQ Integration**: Messaging is handled via RabbitMQ for scalability.
3. **Player Analytics**: Track top spenders and player activity with stored procedures.
4. **Global Exception Handling**: Ensures consistent error responses for the client.

## API Endpoints

- `POST /api/player/casinowager`: Submit a new casino wager.
- `GET /api/player/{playerId}/casino`: Retrieve wagers for a specific player.
- `GET /api/player/topSpenders`: Retrieve top spenders.

## Error Handling
Errors are handled globally using middleware, which returns custom error responses in a consistent format.

