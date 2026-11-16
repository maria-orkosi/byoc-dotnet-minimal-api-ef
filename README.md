# Inventory (.NET minimal API + EF Core)

Sample repository 6 of the Orkosi bring-your-code test matrix. One backend component in C#: Kestrel on `PORT`
(8080 by default), health at `/health`, EF Core migrations in `Migrations/` (history table `__EFMigrationsHistory`),
release command `dotnet ef database update`.
