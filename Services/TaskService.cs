using System.Collections.Concurrent;
using TaskPulse.Models;
using TaskStatus = TaskPulse.Models.TaskStatus;

namespace TaskPulse.Services
{
    public class TaskService : ITaskService
    {
        private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
        private int _nextId = 1;

        public TaskService()
        {
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            var seedTasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Set up ASP.NET Core Project Architecture",
                    Description = "Configure dependency injection, MVC routing, and API controllers for the web app.",
                    Category = "Backend",
                    Priority = TaskPriority.Urgent,
                    Status = TaskStatus.Completed,
                    DueDate = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    CompletedAt = DateTime.UtcNow.AddDays(-1)
                },
                new TaskItem
                {
                    Title = "Design Dynamic Glassmorphic CSS Dashboard",
                    Description = "Implement modern styling, badge indicators, modal dialogs, and dark/light responsive layout.",
                    Category = "Frontend",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(2),
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new TaskItem
                {
                    Title = "Implement REST API Controller Endpoints",
                    Description = "Expose GET, POST, PUT, DELETE, and filter endpoints for task items under /api/tasks.",
                    Category = "Backend",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(1),
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new TaskItem
                {
                    Title = "Write Unit Tests & Verification Suite",
                    Description = "Test CRUD methods and verify stats calculator precision for dashboard metrics.",
                    Category = "QA & Testing",
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Todo,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new TaskItem
                {
                    Title = "Prepare Deployment Documentation & Run Guide",
                    Description = "Draft step-by-step instructions for installing .NET SDK and running the app via terminal.",
                    Category = "DevOps",
                    Priority = TaskPriority.Urgent,
                    Status = TaskStatus.Todo,
                    DueDate = DateTime.UtcNow.AddDays(3),
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var task in seedTasks)
            {
                task.Id = _nextId++;
                _tasks[task.Id] = task;
            }
        }

        public IEnumerable<TaskItem> GetAllTasks(string? status = null, string? priority = null, string? category = null, string? search = null)
        {
            var query = _tasks.Values.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskStatus>(status, true, out var statusEnum))
            {
                query = query.Where(t => t.Status == statusEnum);
            }

            if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<TaskPriority>(priority, true, out var priorityEnum))
            {
                query = query.Where(t => t.Priority == priorityEnum);
            }

            if (!string.IsNullOrWhiteSpace(category) && !category.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLowerInvariant();
                query = query.Where(t => t.Title.ToLowerInvariant().Contains(term) || t.Description.ToLowerInvariant().Contains(term));
            }

            return query.OrderByDescending(t => t.Priority).ThenBy(t => t.DueDate ?? DateTime.MaxValue);
        }

        public TaskItem? GetTaskById(int id)
        {
            _tasks.TryGetValue(id, out var task);
            return task;
        }

        public TaskItem CreateTask(TaskItem task)
        {
            task.Id = Interlocked.Increment(ref _nextId);
            task.CreatedAt = DateTime.UtcNow;
            if (task.Status == TaskStatus.Completed && task.CompletedAt == null)
            {
                task.CompletedAt = DateTime.UtcNow;
            }

            _tasks[task.Id] = task;
            return task;
        }

        public TaskItem? UpdateTask(int id, TaskItem updatedTask)
        {
            if (!_tasks.TryGetValue(id, out var existingTask))
            {
                return null;
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.Category = updatedTask.Category;
            existingTask.Priority = updatedTask.Priority;

            if (existingTask.Status != TaskStatus.Completed && updatedTask.Status == TaskStatus.Completed)
            {
                existingTask.CompletedAt = DateTime.UtcNow;
            }
            else if (updatedTask.Status != TaskStatus.Completed)
            {
                existingTask.CompletedAt = null;
            }

            existingTask.Status = updatedTask.Status;
            existingTask.DueDate = updatedTask.DueDate;

            return existingTask;
        }

        public bool DeleteTask(int id)
        {
            return _tasks.TryRemove(id, out _);
        }

        public TaskItem? ToggleTaskStatus(int id)
        {
            if (!_tasks.TryGetValue(id, out var task))
            {
                return null;
            }

            if (task.Status == TaskStatus.Completed)
            {
                task.Status = TaskStatus.Todo;
                task.CompletedAt = null;
            }
            else
            {
                task.Status = TaskStatus.Completed;
                task.CompletedAt = DateTime.UtcNow;
            }

            return task;
        }

        public DashboardStats GetDashboardStats()
        {
            var all = _tasks.Values.ToList();
            int total = all.Count;
            int completed = all.Count(t => t.Status == TaskStatus.Completed);
            int inProgress = all.Count(t => t.Status == TaskStatus.InProgress);
            int todo = all.Count(t => t.Status == TaskStatus.Todo);
            int urgent = all.Count(t => t.Priority == TaskPriority.Urgent && t.Status != TaskStatus.Completed);
            double pct = total > 0 ? Math.Round((double)completed / total * 100, 1) : 0;

            return new DashboardStats
            {
                TotalTasks = total,
                TodoTasks = todo,
                InProgressTasks = inProgress,
                CompletedTasks = completed,
                UrgentTasks = urgent,
                CompletionPercentage = pct
            };
        }

        public IEnumerable<string> GetCategories()
        {
            return _tasks.Values
                .Select(t => t.Category)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(c => c);
        }
    }
}
