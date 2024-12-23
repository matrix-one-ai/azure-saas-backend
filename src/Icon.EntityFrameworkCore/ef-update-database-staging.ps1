# Prompt for user input
$userinput = Read-Host "You are about to update the STAGING database, are you sure? (yes/no)"

# Check user input and exit if not 'yes'
if ($userinput -ine "yes") {
    Write-Host "Aborting..."
    exit
}

# Navigate directories and set environment variables
Set-Location ..\Icon.Web.Host\
$env:ASPNETCORE_ENVIRONMENT = "Staging"
Write-Host "Temp Host Environment:" $env:ASPNETCORE_ENVIRONMENT

Set-Location ..\Icon.EntityFrameworkCore\
$env:ASPNETCORE_ENVIRONMENT = "Staging"
Write-Host "Temp EF Core Environment:" $env:ASPNETCORE_ENVIRONMENT

# Execute dotnet command
dotnet ef database update --configuration Release --context IconDbContext --project ".\Icon.EntityFrameworkCore.csproj" --startup-project "..\Icon.Web.Host\Icon.Web.Host.csproj"

# Navigate back and reset environment variable
Set-Location ..\Icon.Web.Host\
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "Current Host Environment:" $env:ASPNETCORE_ENVIRONMENT

Set-Location ..\Icon.EntityFrameworkCore\
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "Current EF Core Environment:" $env:ASPNETCORE_ENVIRONMENT
