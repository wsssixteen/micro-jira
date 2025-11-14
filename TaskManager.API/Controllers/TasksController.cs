using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            _logger.LogInformation("➡️ GET /api/tasks called - fetching tasks from database...");

            try
            {
                var tasks = await _context.Tasks.ToListAsync();
                _logger.LogInformation("✔️ Loaded {Count} tasks from DB", tasks.Count);
                return tasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ERROR loading tasks from DB");
                return StatusCode(500, "Failed to load tasks");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _logger.LogInformation(
            "GET /api/tasks at {Time}: count={Count}, ids={Ids}",
            DateTime.UtcNow,
            task.Id,
            string.Join(",", task.IsComplete));

            return task;
        }
        
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/toggle-complete")]
        public async Task<ActionResult<TaskItem>> ToggleComplete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.IsComplete = !task.IsComplete;
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("reset-demo")]
        public async Task<IActionResult> ResetDemoData()
        {

            // Clear existing tasks
            _context.Tasks.RemoveRange(_context.Tasks);
            await _context.SaveChangesAsync();

            // Re-seed demo tasks
            var now = DateTime.UtcNow;
            var demo = new List<TaskItem> {
                new TaskItem { Title = "Design PostgreSQL schema with users, projects, and tasks tables", Description = "Create normalized database schema for scalable task management", IsComplete = false, CreatedAt = now.AddDays(-15) },
                new TaskItem { Title = "Implement JWT authentication in ASP.NET Core API", Description = "Add token-based authentication with refresh token rotation", IsComplete = false, CreatedAt = now.AddDays(-12) },
                new TaskItem { Title = "Build Docker image and push to Azure Container Registry", Description = "Create multi-stage Dockerfile and configure ACR integration", IsComplete = false, CreatedAt = now.AddDays(-8) },
                new TaskItem { Title = "[URGENT] Fix: POST /api/tasks returns 500 on invalid input - add validation", Description = "Add FluentValidation and input sanitization to prevent errors", IsComplete = false, CreatedAt = now.AddDays(-3) },
                new TaskItem { Title = "Write unit tests for TasksController CRUD endpoints", Description = "Achieve 85%+ code coverage with xUnit and Moq", IsComplete = true, CreatedAt = now.AddDays(-10) },
                new TaskItem { Title = "Configure staging environment variables and deploy to AKS cluster", Description = "Set up ConfigMaps, secrets, and Helm charts for AKS deployment", IsComplete = true, CreatedAt = now.AddDays(-5) }
            };
            
            _context.Tasks.AddRange(demo);
            await _context.SaveChangesAsync();

            return Ok("Demo tasks reset successfully.");
        }
    }
}