# 🦸‍♂️ SerBeast Backend

**SerBeast Backend** provides the core logic and API endpoints for the **SerBeast** platform. It is built using **ASP.NET** and communicates with a **SQL Server** database to handle user data, service listings, and other essential functionalities.

---

## 🔗 Frontend Repository

The front-end for **SerBeast** communicates with the API provided by this repository.

- [SerBeast Frontend Repository](https://github.com/angelobracero/SerBeast)

---

## 🌟 Features

- **🔍 API Endpoints**: Provides endpoints to search for professionals, get service listings, and manage users.
- **📋 Service Management**: Allows CRUD operations for services.

---

## 🚀 Technologies Used

- **ASP.NET Core**: Backend framework for building RESTful APIs.
- **SQL Server**: Database for storing service and user information.
- **Entity Framework Core**: ORM for managing database operations.

---

## 📂 File Structure

```plaintext
├── /SerBeast.API
│   ├── /Controllers        # API controllers (Service, User, etc.)
│   ├── /Data               # Database context and models
│   ├── /Migrations         # EF migrations for database schema changes
│   ├── /Model              # Data models, DTOs, or business logic
│   ├── /Uploads            # Directory for uploaded files (e.g., images)
│   ├── appsettings.json    # Configuration settings
│   └── Program.cs          # Main entry point for the application
└── /SerBeast.Utility       # Helper classes and utilities
```

## 🛠️ Setup and Installation

### Prerequisites
- Visual Studio or Visual Studio Code
- .NET SDK (>= 8.x)
- SQL Server (Local or Remote)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/angelobracero/SerBeast_API.git
   cd SerBeast_API
   ```
2. Install dependencies:
   - Open the solution in **Visual Studio** or **Visual Studio Code**.
   - Ensure the required NuGet packages are restored by running:
     
     ```bash
     dotnet restore
     ```

3. Set up the database:
   - Make sure you have a **SQL Server** database set up.
   - Update the `ConnectionStrings` in `appsettings.json` to point to your SQL Server instance.

4. Run database migrations:
   - Apply migrations to the database using the following command:
     
     ```bash
     dotnet ef database update
     ```

5. Run the API:
   - Start the API using **Visual Studio** or run the following command:
     
     ```bash
     dotnet run
     ```

6. Visit the API in your browser:
   ```plaintext
   http://localhost:5000
   ```

