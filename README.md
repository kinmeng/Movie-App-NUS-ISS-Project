# Community-driven Movie App (NUS-ISS)
This is a community-driven movie app built as part of NUS-ISS Project

Prerequisites <br />
Visual Studio 2022
SQL Server Management Studio

Project setup
1. Create a new database in SSMS of your preferred database name
2. Setup your appsettings.json with the appropriate credentials - connectionStrings and API Key


  <b>Run these commands</b> <br />
  dotnet tool install --global dotnet-ef <br />
  dotnet ef migrations add InitialCreate <br />
  dotnet ef database update <br />

3. Try running the project 
