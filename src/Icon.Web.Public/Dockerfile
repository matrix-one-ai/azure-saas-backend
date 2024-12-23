#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Icon.Web.Public/Icon.Web.Public.csproj", "src/Icon.Web.Public/"]
COPY ["src/Icon.Web.Core/Icon.Web.Core.csproj", "src/Icon.Web.Core/"]
COPY ["src/Icon.Application/Icon.Application.csproj", "src/Icon.Application/"]
COPY ["src/Icon.Application.Shared/Icon.Application.Shared.csproj", "src/Icon.Application.Shared/"]
COPY ["src/Icon.Core.Shared/Icon.Core.Shared.csproj", "src/Icon.Core.Shared/"]
COPY ["src/Icon.Core/Icon.Core.csproj", "src/Icon.Core/"]
COPY ["src/Icon.EntityFrameworkCore/Icon.EntityFrameworkCore.csproj", "src/Icon.EntityFrameworkCore/"]
COPY ["src/Icon.GraphQL/Icon.GraphQL.csproj", "src/Icon.GraphQL/"]
RUN dotnet restore "src/Icon.Web.Public/Icon.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/Icon.Web.Public"
RUN dotnet build "Icon.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Icon.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Icon.Web.Public.dll"]
