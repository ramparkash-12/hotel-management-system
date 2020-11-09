FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
# Install NodeJs
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
# End Install
WORKDIR /src
#COPY "/Services/Hotel/Hotel.API/Hotel.API.csproj"  "/Services/Hotel/Hotel.API/Hotel.API.csproj"
COPY "/Web/WebSPA/WebSPA.csproj" "/Web/WebSPA/WebSPA.csproj"
#RUN dotnet restore --no-cache "WebSPA.csproj"
COPY . .
#WORKDIR /src
WORKDIR "/src/Web/WebSPA/"
RUN dotnet build "WebSPA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSPA.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim as base
# Install NodeJs
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
# End Install
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSPA.dll"]