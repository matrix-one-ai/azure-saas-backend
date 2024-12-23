@echo off
set /p name="Name: "
dotnet ef migrations add %name% --configuration Release --context IconDbContext --project ".\Icon.EntityFrameworkCore.csproj" --startup-project "..\Icon.Web.Host\Icon.Web.Host.csproj"
