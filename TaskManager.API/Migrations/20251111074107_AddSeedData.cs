using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Tasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "53, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "IsComplete", "Title" },
                values: new object[,]
                {
                    { 47, new DateTime(2025, 10, 27, 7, 41, 7, 116, DateTimeKind.Utc).AddTicks(9859), "Create normalized database schema for scalable task management", false, "Design PostgreSQL schema with users, projects, and tasks tables" },
                    { 48, new DateTime(2025, 10, 30, 7, 41, 7, 117, DateTimeKind.Utc).AddTicks(146), "Add token-based authentication with refresh token rotation", false, "Implement JWT authentication in ASP.NET Core API" },
                    { 49, new DateTime(2025, 11, 3, 7, 41, 7, 117, DateTimeKind.Utc).AddTicks(151), "Create multi-stage Dockerfile and configure ACR integration", false, "Build Docker image and push to Azure Container Registry" },
                    { 50, new DateTime(2025, 11, 8, 7, 41, 7, 117, DateTimeKind.Utc).AddTicks(153), "Add FluentValidation and input sanitization to prevent errors", false, "[URGENT] Fix: POST /api/tasks returns 500 on invalid input - add validation" },
                    { 51, new DateTime(2025, 11, 1, 7, 41, 7, 117, DateTimeKind.Utc).AddTicks(154), "Achieve 85%+ code coverage with xUnit and Moq", true, "Write unit tests for TasksController CRUD endpoints" },
                    { 52, new DateTime(2025, 11, 6, 7, 41, 7, 117, DateTimeKind.Utc).AddTicks(156), "Set up ConfigMaps, secrets, and Helm charts for AKS deployment", true, "Configure staging environment variables and deploy to AKS cluster" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Tasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "53, 1");
        }
    }
}
