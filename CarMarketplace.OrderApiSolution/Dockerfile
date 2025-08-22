FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY .. .
WORKDIR /src/CarMarketplace.OrderApiSolution/OrderApi.Presentation
RUN dotnet publish OrderApi.Presentation.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OrderApi.Presentation.dll"]