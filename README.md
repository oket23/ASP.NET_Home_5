# 👥 Users & Subscriptions Management API (Clean Architecture)

A structured, educational, and extensible ASP.NET Core Web API designed to manage users and their service subscriptions.

The project demonstrates essential enterprise practices such as **Layered Architecture**, **Entity Framework Core** integration with complex querying, **Custom Middleware Pipelines**, and **DTO-based communication** to ensure clean and scalable code.

## 🚀 Technologies Used

* **.NET 9**
* **ASP.NET Core Web API**
* **Entity Framework Core** (SQL Server)
* **C# 12**
* **Serilog** (Structured Logging)
* **Swagger / OpenAPI**
* **Layered Architecture**: API → BLL (Services) → DAL (Repositories)
* **Asynchronous Programming**: `async/await` patterns

## 📋 Features Overview

This API provides complete CRUD functionality for Users and Subscriptions, advanced filtering capabilities, and a robust middleware pipeline for monitoring and security.

### ⭐ Key Features

#### 1. Custom Middleware Pipeline
The application implements a sophisticated request processing pipeline manually built for educational purposes:

* **⏱️ Metrics Middleware:** Measures execution time for every request, logs it via Serilog, and wraps the response in a standardized JSON envelope containing metadata (Time + "Copyright").
* **🛡️ Fake Auth Middleware:** Secures modifying endpoints (`POST`, `PUT`, `DELETE`). It inspects headers for a specific `Token` and returns `403 Forbidden` if it's missing or invalid.
* **⚠️ Global Exception Handler:** A centralized error catching mechanism that converts unhandled exceptions into standardized JSON error responses, preventing sensitive data leakage.

*Configured in `Program.cs`:*

```csharp
app.UseMiddleware<MetricsMiddleware>();       // Measurements & Wrapper
app.UseMiddleware<GlobalExceptionHandler>();  // Error Handling
app.UseMiddleware<FakeAuthMiddleware>();      // Security Check
```
#### 2. Advanced Search & Filtering
Instead of creating dozens of separate endpoints, the API uses **Filter Objects** passed via Query String:
* **Pagination:** Standardized `PageNumber` and `PageSize` handling.
* **Dynamic Filtering:** * *Users:* Filter by name prefix, subscription presence.
    * *Subscriptions:* Filter by type, price range, expiration status.

#### 3. Complex Business Logic (EF Core)
The DAL layer implements specific analytical queries using efficient LINQ translations:
* **Aggregation:** Grouping subscriptions by type and counting unique users (`GroupBy`).
* **Statistical Analysis:** Finding subscriptions priced above the average (`Average`).
* **Optimized Projections:** Fetching only required data fields.

#### 4. Safe DTO Mapping
To prevent **Cyclic Reference Errors** (User -> Subscriptions -> User), the project uses a clean DTO pattern:
* **Entities** are kept inside the Domain layer.
* **Response DTOs** (`UserResponse`, `SubscriptionResponse`) are returned to the client.
* Mapping is handled via extension methods to ensure separation of concerns.

## 🧩 Project Structure

The solution follows a strict **Clean Architecture** approach, separating concerns into three distinct projects:

```text
Home_5.sln
│
├── Home_5.API                      # Presentation Layer (Entry Point)
│   ├── Controllers/                # API Endpoints (Users, Subscriptions)
│   ├── Extensions/                 # Extension methods (DTO Mappers)
│   ├── Interfaces/                 # Service Interfaces
│   ├── Middleware/                 # Custom Pipeline (Metrics, Auth, Exceptions)
│   ├── Requests/                   # Input DTOs (Create/Update models)
│   ├── Responses/                  # Output DTOs (Safe JSON responses)
│   ├── Services/                   # Business Logic Implementations
│   ├── Validations/                # Business Rules Validation
│   ├── DependencyInjection.cs      # DI Container Setup
│   └── Program.cs                  # App Configuration
│
├── Home_5.BLL                      # Business Logic Layer (Core Domain)
│   ├── Enums/                      # Enumerations (SubscriptionsEnum)
│   ├── Filters/                    # Filter Models for Search
│   ├── Interfaces/                 # Repository Interfaces
│   └── Models/                     # Domain Entities (User, Subscription)
│
└── Home_5.DAL                      # Data Access Layer (Infrastructure)
    ├── Configurations/             # EF Core Fluent API Configurations
    ├── Migrations/                 # Database Schema History
    ├── Repositories/               # Data Access Logic
    └── HomeContext.cs              # Entity Framework DbContext
```

## 🛠 Getting Started

### 1. Clone the repository
```bash
git clone [https://github.com/your-username/Home_5.git](https://github.com/your-username/Home_5.git)
```

### 2. Navigate to the project
```bash
cd Home_5/Home_5.API
```
### 3. Install EF Core tools (if needed)
If you haven't used Entity Framework Core before, install the global tool:
```bash
dotnet tool install --global dotnet-ef
```
## ⚙️ Configuration (SQL Server)

Ensure your connection string is set in `Home_5.API/appsettings.json`. The default configuration uses **LocalDB**, which is installed with Visual Studio:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Home5Db;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
## 🔄 Database Migrations

Since this project uses Entity Framework Core with a **Code-First** approach, you need to apply migrations to create the database schema.

**1. Create the database & apply tables:**
Run this command from the solution folder:

```bash
dotnet ef database update --project Home_5.DAL --startup-project Home_5.API
```

## ▶️ Running the Application

Run the API from the terminal:
```bash
dotnet run --project Home_5.API
```

The application will start, and you will see the listening ports in the console (usually `https://localhost:7041` or `http://localhost:5xxx`).

## 🧪 Testing & Security

### Swagger UI
The project includes Swagger for API documentation and interactive testing.
* Open your browser and go to: `https://localhost:7041/swagger` (check your console for the exact port).

### 🛡 Security (Fake Auth)
The `FakeAuthMiddleware` protects data modification endpoints (`POST`, `PUT`, `DELETE`). 

To successfully perform these requests, you **MUST** add the following header (in Postman or Swagger):

```text
Token: 1234
```

*Requests without this token header will receive a `403 Forbidden` response.*

## 📁 Endpoints Overview

### 👤 Users
Base path: `/v1/users`

| Method | Route | Description |
| :--- | :--- | :--- |
| `GET` | `/` | Get all users (Supports filters: `firstNameStartsWith`, `hasSubscriptions`) |
| `GET` | `/{id}` | Get user by ID |
| `POST` | `/` | Create a new user (Requires Token) |
| `PUT` | `/{id}` | Update user details (Requires Token) |
| `DELETE` | `/{id}` | Delete user (Requires Token) |
| `GET` | `/most-subscriptions` | Get user with the highest number of subscriptions |
| `GET` | `/premium-names` | Get names of top 5 users with Premium subscription |

### 💳 Subscriptions
Base path: `/v1/subscriptions`

| Method | Route | Description |
| :--- | :--- | :--- |
| `GET` | `/` | Get all subscriptions (Filter by: `type`, `isExpired`, `price`) |
| `GET` | `/{id}` | Get subscription by ID |
| `POST` | `/` | Create a new subscription (Requires Token) |
| `PUT` | `/{id}` | Update subscription (Requires Token) |
| `DELETE` | `/{id}` | Delete subscription (Requires Token) |
| `GET` | `/expensive` | Get subscriptions with price higher than global average |
| `GET` | `/count-by-type` | Get total count grouped by subscription type |
| `GET` | `/users-count-by-type` | Get count of unique users grouped by subscription type |

## 🎓 Educational Value

This project demonstrates:
* **Middleware Pattern**: How to intercept HTTP requests for logging (`MetricsMiddleware`) and security (`FakeAuthMiddleware`).
* **Clean Architecture**: Decoupling Business Logic (`BLL`) from Data Access (`DAL`) and API presentation.
* **EF Core Mastery**: Using `GroupBy`, `Average`, and complex `Where` clauses efficiently on the server side.
* **API Best Practices**: Using **DTOs** to shape responses (avoiding circular references) and handling Enums as strings (`JsonStringEnumConverter`) for better readability.

## 📜 License

This project is intended for educational use. You may freely modify or extend it for your own learning.
