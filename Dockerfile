FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["/Foundation/EventBus/EventBus/EventBus.csproj", "/Foundation/EventBus/EventBus/EventBus.csproj"]
COPY ["/Foundation/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "/Foundation/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"]

COPY "/Services/Identity/Identity.API/Identity.API.csproj"  "/Services/Identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]  