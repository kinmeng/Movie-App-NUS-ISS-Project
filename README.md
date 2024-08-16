# Community-driven Movie App (NUS-ISS)
This is a community-driven movie app built as part of NUS-ISS Project

Prerequisites
Visual Studio 2022
SQL Server Management Studio

Project setup
1. Create a new database in SSMS of your preferredName
2. Setup your appsettings.json with the appropriate credentials - connectionStrings and API Key
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
Try running the project 
