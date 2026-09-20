# RentalCar.Api

## Requirements
- .NET 6 SDK
- PostgreSQL

## Run
1. Update the connection string and JWT settings in `appsettings.json`.
2. Create the database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
3. Start the API:
   ```bash
   dotnet run
   ```

Swagger will be available at the HTTPS URL shown in the terminal.
