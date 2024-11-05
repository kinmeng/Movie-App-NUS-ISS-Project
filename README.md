# Community-driven Movie App (NUS-ISS)
This is a community-driven movie app built as part of NUS-ISS Project

Prerequisites <br />
Visual Studio 2022
SQL Server Management Studio

Project setup
1. Clone the project
2. Create a new database in SSMS of your preferred database name
3. Setup your `appsettings.json` with the appropriate credentials - connectionStrings and API Key
4. Run these commands <br />
  ```dotnet tool install --global dotnet-ef``` <br />
  ```dotnet ef migrations add InitialCreate``` (optional) <br />
  ```dotnet ef database update``` <br />

5. Try running the project (IIS Express)
