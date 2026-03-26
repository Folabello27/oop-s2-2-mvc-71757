# Food Safety Inspection Tracker

ASP.NET Core MVC app for tracking premises inspections, outcomes, and follow-ups with role-based access and audit-friendly logging.

## Tech Stack
- ASP.NET Core MVC
- EF Core + SQLite
- Identity Roles
- Serilog (console + rolling file)
- xUnit tests
- GitHub Actions CI

## Quick Start
1. Restore and build:
   ```bash
   dotnet build --configuration Release
   ```
2. Run the app:
   ```bash
   dotnet run --project .\oop-s2-2-mvc-71757\oop-s2-2-mvc-71757.csproj
   ```

## Default Accounts (Seeded)
These are created on startup by the identity seeder:
- Admin: `admin@foodsafety.local` / `Admin123!`
- Inspector: `inspector@foodsafety.local` / `Inspector123!`
- Viewer: `viewer@foodsafety.local` / `Viewer123!`

The credentials are defined in:
- [IdentitySeeder.cs](Data/IdentitySeeder.cs)

## Logging
Serilog writes to console and rolling files:
- Logs directory: `oop-s2-2-mvc-71757\Logs`
- Enriched properties: `Application`, `Environment`, `UserName`

## Tests
```bash
dotnet test --configuration Release
```

## Dashboard
The `/Dashboard` page shows:
- Inspections this month
- Failed inspections this month
- Overdue open follow-ups
With filters by Town and RiskRating.
