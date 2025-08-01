FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything into the container
COPY . .

# Restore and publish
WORKDIR /src/AuthenticationApi.Presentation
RUN dotnet publish AuthenticationApi.Presentation.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AuthenticationApi.Presentation.dll"]