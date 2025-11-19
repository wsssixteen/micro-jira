using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core DbContext with PostgreSQL (Railway)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(conn);
});

// Allow CORS for frontend
builder.Services.AddCors(options =>
{
    var allowedOrigins = new List<string> { "https://lemon-water-02d583710.3.azurestaticapps.net" };
    
    if (builder.Environment.IsDevelopment())
    {
        allowedOrigins.Add("http://localhost:4200");
    }
    
    options.AddPolicy("AllowAll", policy =>
        policy.WithOrigins(allowedOrigins.ToArray())
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();
 
// Runtime seeding: if Tasks table is empty, insert demo tasks so the app always shows content
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        // Apply any pending migrations
        db.Database.Migrate();

        if (!db.Tasks.Any())
        {
            var now = DateTime.UtcNow;
            var demo = new List<TaskItem>
            {
                new TaskItem { Title = "Design PostgreSQL schema with users, projects, and tasks tables", Description = "Create normalized database schema for scalable task management", IsComplete = false, CreatedAt = now.AddDays(-15) },
                new TaskItem { Title = "Implement JWT authentication in ASP.NET Core API", Description = "Add token-based authentication with refresh token rotation", IsComplete = false, CreatedAt = now.AddDays(-12) },
                new TaskItem { Title = "Build Docker image and push to Azure Container Registry", Description = "Create multi-stage Dockerfile and configure ACR integration", IsComplete = false, CreatedAt = now.AddDays(-8) },
                new TaskItem { Title = "[URGENT] Fix: POST /api/tasks returns 500 on invalid input - add validation", Description = "Add FluentValidation and input sanitization to prevent errors", IsComplete = false, CreatedAt = now.AddDays(-3) },
                new TaskItem { Title = "Write unit tests for TasksController CRUD endpoints", Description = "Achieve 85%+ code coverage with xUnit and Moq", IsComplete = true, CreatedAt = now.AddDays(-10) },
                new TaskItem { Title = "Configure staging environment variables and deploy to AKS cluster", Description = "Set up ConfigMaps, secrets, and Helm charts for AKS deployment", IsComplete = true, CreatedAt = now.AddDays(-5) }
            };

            db.Tasks.AddRange(demo);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // If seeding fails, write to console but continue — useful for diagnostics in deployment
        Console.WriteLine($"Error during seeding: {ex.Message}");
    }
}
// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.Use((context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    return next();
});

app.MapControllers();

app.Run();
