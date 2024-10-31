FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["../Helpers/Helpers.csproj", "Helpers/"]
COPY ["../Soundify.DAL.PostgreSQL/Soundify.DAL.PostgreSQL.csproj", "Soundify.DAL.PostgreSQL/"]
COPY ["Soundify/Soundify.csproj", "Soundify/"]

RUN dotnet restore "Soundify/Soundify.csproj"

COPY . .

WORKDIR "/src/Soundify"
RUN dotnet build "Soundify.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Soundify.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Soundify.dll"]
