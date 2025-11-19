using Microsoft.EntityFrameworkCore;
using TaskManager.API.Models;

namespace TaskManager.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data - Starting from ID 47
            modelBuilder.Entity<TaskItem>().HasData(
                // Active tasks (4)
                new TaskItem 
                { 
                    Id = 47, 
                    Title = "Design PostgreSQL schema with users, projects, and tasks tables",
                    Description = "Create normalized database schema for scalable task management",
                    IsComplete = false,
                    CreatedAt = new DateTime(2025, 10, 27, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem 
                { 
                    Id = 48, 
                    Title = "Implement JWT authentication in ASP.NET Core API",
                    Description = "Add token-based authentication with refresh token rotation",
                    IsComplete = false,
                    CreatedAt = new DateTime(2025, 10, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem 
                { 
                    Id = 49, 
                    Title = "Build Docker image and push to Azure Container Registry",
                    Description = "Create multi-stage Dockerfile and configure ACR integration",
                    IsComplete = false,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem 
                { 
                    Id = 50, 
                    Title = "[URGENT] Fix: POST /api/tasks returns 500 on invalid input - add validation",
                    Description = "Add FluentValidation and input sanitization to prevent errors",
                    IsComplete = false,
                    CreatedAt = new DateTime(2025, 11, 8, 0, 0, 0, DateTimeKind.Utc)
                },
                // Completed tasks (2)
                new TaskItem 
                { 
                    Id = 51, 
                    Title = "Write unit tests for TasksController CRUD endpoints",
                    Description = "Achieve 85%+ code coverage with xUnit and Moq",
                    IsComplete = true,
                    CreatedAt = new DateTime(2025, 10, 31, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem 
                { 
                    Id = 52, 
                    Title = "Configure staging environment variables and deploy to AKS cluster",
                    Description = "Set up ConfigMaps, secrets, and Helm charts for AKS deployment",
                    IsComplete = true,
                    CreatedAt = new DateTime(2025, 11, 6, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Let the provider handle identity/auto-increment for Id
            modelBuilder.Entity<TaskItem>().Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // Set a database default for CreatedAt (provider-agnostic SQL)
            modelBuilder.Entity<TaskItem>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
