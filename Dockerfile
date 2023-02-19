FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/OtelApp/OtelApp.csproj", "src/OtelApp/"]
RUN dotnet restore "src/OtelApp/OtelApp.csproj"
COPY . .
RUN dotnet build "src/OtelApp/OtelApp.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "src/OtelApp/OtelApp.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet "OtelApp.dll"
