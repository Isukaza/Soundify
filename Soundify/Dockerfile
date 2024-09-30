FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8433

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

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
