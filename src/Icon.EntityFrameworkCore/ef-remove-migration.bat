@echo off
dotnet ef migrations remove --configuration Release --context IconDbContext --project ".\Icon.EntityFrameworkCore.csproj" --startup-project "..\Icon.Web.Host\Icon.Web.Host.csproj"
