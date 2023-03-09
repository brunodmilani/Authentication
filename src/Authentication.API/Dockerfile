FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers
COPY ./Authentication.sln .
COPY ./src/Authentication.API/Authentication.API.csproj ./src/Authentication.API/
COPY ./src/Authentication.Data/Authentication.Data.csproj ./src/Authentication.Data/
COPY ./src/Authentication.Domain/Authentication.Domain.csproj ./src/Authentication.Domain/
COPY ./src/Authentication.Shared/Authentication.Shared.csproj ./src/Authentication.Shared/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish --no-restore -c Release -o out ./src/Authentication.API

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["./Authentication.API"]