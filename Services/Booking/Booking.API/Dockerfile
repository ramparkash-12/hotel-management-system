FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["/Foundation/EventBus/EventBus/EventBus.csproj", "/Foundation/EventBus/EventBus/EventBus.csproj"]
COPY ["/Foundation/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "/Foundation/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"]

COPY "/Services/Booking/Booking.API/Booking.API.csproj"  "/Services/Booking/Booking.API/Booking.API.csproj"
COPY . .
WORKDIR "/src/Services/Booking/Booking.API"
RUN dotnet build "Booking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Booking.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.API.dll"]  

