
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy everything and restore dependencies
COPY . .
RUN dotnet restore "EcoTrack.Api/EcoTrack.Api.csproj"

# Build and publish
RUN dotnet publish "EcoTrack.Api/EcoTrack.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the API runs on
EXPOSE 5050
ENV ASPNETCORE_URLS=http://+:5050

ENTRYPOINT ["dotnet", "EcoTrack.Api.dll"]