#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Icon.Migrator/Icon.Migrator.csproj", "src/Icon.Migrator/"]
COPY ["src/Icon.EntityFrameworkCore/Icon.EntityFrameworkCore.csproj", "src/Icon.EntityFrameworkCore/"]
COPY ["src/Icon.Core/Icon.Core.csproj", "src/Icon.Core/"]
COPY ["src/Icon.Core.Shared/Icon.Core.Shared.csproj", "src/Icon.Core.Shared/"]
RUN dotnet restore "src/Icon.Migrator/Icon.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Icon.Migrator"
RUN dotnet build "Icon.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Icon.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Icon.Migrator.dll", "-s"]
