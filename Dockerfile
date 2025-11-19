# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file and project file for dependency restore
# The path must be relative to the solution root (4. TASK MANAGER)
COPY ["TaskManager.API/TaskManager.API.csproj", "TaskManager.API/"]
RUN dotnet restore "TaskManager.API/TaskManager.API.csproj"

# Copy all source code and build
COPY . .
WORKDIR /src/TaskManager.API
RUN dotnet publish "TaskManager.API.csproj" -c Release -o /app/publish

# Stage 2: Create the final minimal runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Set the port explicitly
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Set the entry point to run the published DLL
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]