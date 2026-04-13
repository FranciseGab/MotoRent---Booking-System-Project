# MOTORRENT SYSTEM

> A simple motor renting system demonstrating a fullstack implementation using Angular (Frontend) and ASP.NET Core (Backend) with unit tests.

---

## Features

| Feature | Description |
|---------|-------------|
| Signup | Creation of Account |
| Login | User authentication |
| Renting | Select a motor and number of days to rent |
| Transaction | View all rent transactions made |

---

## System Flow

```
User visits site → Signup (if no account) → Login → Renting → Select a motor + number of days → Transaction History
```

---

## Notes

- System only has **users** — no admin role
- In renting, **Selecting Motorcycle to rent**
- Renting only has **Rent Now** option
- Transaction page is **view only** (no edits)

---

## Developers

| Name | Role |
|------|------|
| Eric James Sonio | Backend / Unit Tester / API Developer |
| Francise Grace Gabriel | Frontend / UI/UX Designer |

---

## Frameworks Used

### Frontend — Angular v21.2.0 (Modern / CLI-based)

> Built using modern Angular with Angular CLI. Files follow standalone component conventions (no `.component` suffix).

| Technology | Version | Purpose |
|------------|---------|---------|
| Angular | ^21.2.0 | Frontend Framework |
| TypeScript | ~5.9.2 | Programming Language |
| RxJS | ~7.8.0 | Reactive/Async Operations |
| Jasmine + Karma | ^6.1.0 / ^6.4.4 | Unit Testing |
| Vitest | ^4.0.8 | Modern Test Runner |
| Prettier | ^3.8.1 | Code Formatter |
| npm | 11.6.2 | Package Manager |

```
Frontend/
├── src/
│   ├── app/
│   │   ├── apis/                         # API request helpers using environment config
│   │   │   ├── mainApi.ts                # Main helper — uses environment as base API URL
│   │   │   ├── authApi.ts                # Auth requests
│   │   │   ├── rentApi.ts                # Rent requests
│   │   │   └── transactionApi.ts         # Transaction requests
│   │   │
│   │   ├── components/                   # Reusable/helper components
│   │   │   └── navbar/
│   │   │       ├── navbar.html
│   │   │       ├── navbar.ts
│   │   │       └── navbar.css
│   │   │
│   │   ├── models/                       # TypeScript interfaces/models
│   │   │   ├── auth.model.ts
│   │   │   ├── rent.model.ts
│   │   │   └── transaction.model.ts
│   │   │
│   │   ├── pages/
│   │   │   ├── login/
│   │   │   │   ├── login.html
│   │   │   │   ├── login.ts              # Uses AuthService
│   │   │   │   ├── login.spec.ts         # Unit tests
│   │   │   │   └── login.css
│   │   │   ├── signup/
│   │   │   │   ├── signup.html
│   │   │   │   ├── signup.ts             # Uses AuthService
│   │   │   │   └── signup.css
│   │   │   ├── renting/
│   │   │   │   ├── renting.html
│   │   │   │   ├── renting.ts
│   │   │   │   ├── renting.spec.ts       # Unit tests
│   │   │   │   └── renting.css
│   │   │   └── transactions/
│   │   │       ├── transactions.html
│   │   │       ├── transactions.ts
│   │   │       ├── transaction.spec.ts   # Unit tests
│   │   │       └── transactions.css
│   │   │
│   │   ├── services/                     # Business logic layer
│   │   │   ├── auth.service.ts
│   │   │   ├── rent.service.ts
│   │   │   └── transaction.service.ts
│   │   │
│   │   ├── app.config.ts                 # App configuration
│   │   ├── app.routes.ts                 # Route definitions
│   │   ├── app.ts
│   │   ├── app.html
│   │   └── app.css
│   │
│   ├── environments/                     # Environment config
│   │   ├── environment.ts                # Dev — Base API URL: http://localhost:5185
│   │   └── environment.prod.ts           # Production config
│   │
│   ├── index.html
│   ├── main.ts
│   ├── styles.css
│   └── test.ts
```

---

### Backend — ASP.NET Core (.NET 9)

> RESTful API built with ASP.NET Core Web API, using Entity Framework Core with SQLite as the database and Swagger for API documentation.

| Technology | Version | Purpose |
|------------|---------|---------|
| ASP.NET Core | .NET 9.0 | Backend Framework |
| Entity Framework Core | 9.0.0 | ORM / Database Access |
| EF Core SQLite | 9.0.0 | Database Provider |
| EF Core Tools | 9.0.0 | Migrations & Scaffolding |
| Swashbuckle (Swagger) | 6.9.0 | API Documentation |

```
Backend/
└── src/
    ├── Controllers/                  # API endpoints
    │   ├── CustomerController.cs
    │   ├── MotorController.cs
    │   └── TransactionController.cs
    │
    ├── Db/                           # Database layer
    │   ├── Models/                   # Entity models
    │   ├── Seeds/                    # Seed data (Motors only)
    │   ├── AppDbContext.cs           # EF Core DB context
    │   ├── DbInit.cs                 # DB initializer
    │   └── db.sqlite                 # SQLite database file
    │
    ├── DTOS/                         # Data Transfer Objects
    │   ├── CustomerDto.cs
    │   └── MotorDto.cs
    │
    ├── Services/                     # Business logic layer
    │   ├── CustomerService.cs
    │   ├── MotorService.cs
    │   └── TransactionService.cs
    │
    └── Properties/
        └── launchSettings.json       # Dev server config (port 5185)
```