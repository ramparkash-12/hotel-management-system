FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["/src/Foundation/EventBus/EventBus/EventBus.csproj", "/src/Foundation/EventBus/EventBus/EventBus.csproj"]
COPY ["/Foundation/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "/Foundation/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"]

COPY Identity.API.csproj .
COPY . .
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]  