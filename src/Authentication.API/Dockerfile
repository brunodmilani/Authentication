#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Authentication.API/Authentication.API.csproj", "src/Authentication.API/"]
COPY ["src/Authentication.Shared/Authentication.Shared.csproj", "src/Authentication.Shared/"]
COPY ["src/Authentication.Data/Authentication.Data.csproj", "src/Authentication.Data/"]
COPY ["src/Authentication.Domain/Authentication.Domain.csproj", "src/Authentication.Domain/"]
RUN dotnet restore "src/Authentication.API/Authentication.API.csproj"
COPY . .
WORKDIR "/src/src/Authentication.API"
RUN dotnet build "Authentication.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.API.dll"]