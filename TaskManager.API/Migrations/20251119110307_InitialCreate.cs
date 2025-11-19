using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "IsComplete", "Title" },
                values: new object[,]
                {
                    { 47, new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Create normalized database schema for scalable task management", false, "Design PostgreSQL schema with users, projects, and tasks tables" },
                    { 48, new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Add token-based authentication with refresh token rotation", false, "Implement JWT authentication in ASP.NET Core API" },
                    { 49, new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Create multi-stage Dockerfile and configure ACR integration", false, "Build Docker image and push to Azure Container Registry" },
                    { 50, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Add FluentValidation and input sanitization to prevent errors", false, "[URGENT] Fix: POST /api/tasks returns 500 on invalid input - add validation" },
                    { 51, new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Achieve 85%+ code coverage with xUnit and Moq", true, "Write unit tests for TasksController CRUD endpoints" },
                    { 52, new DateTime(2025, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Set up ConfigMaps, secrets, and Helm charts for AKS deployment", true, "Configure staging environment variables and deploy to AKS cluster" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
