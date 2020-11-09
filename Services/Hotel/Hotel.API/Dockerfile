FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

#COPY Hotel.API.csproj .
COPY "/Services/Hotel/Hotel.API/Hotel.API.csproj"  "/Services/Hotel/Hotel.API/Hotel.API.csproj"
COPY . .
WORKDIR "/src/Services/Hotel/Hotel.API"
RUN dotnet build "Hotel.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotel.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotel.API.dll"]