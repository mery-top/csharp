using TaskPulse.Models;

namespace TaskPulse.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAllTasks(string? status = null, string? priority = null, string? category = null, string? search = null);
        TaskItem? GetTaskById(int id);
        TaskItem CreateTask(TaskItem task);
        TaskItem? UpdateTask(int id, TaskItem updatedTask);
        bool DeleteTask(int id);
        TaskItem? ToggleTaskStatus(int id);
        DashboardStats GetDashboardStats();
        IEnumerable<string> GetCategories();
    }
}
