using Microsoft.AspNetCore.Mvc;
using TaskPulse.Models;
using TaskPulse.Services;

namespace TaskPulse.Controllers.Api
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksApiController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get all tasks with optional filters
        /// </summary>
        [HttpGet]
        public IActionResult GetTasks([FromQuery] string? status, [FromQuery] string? priority, [FromQuery] string? category, [FromQuery] string? search)
        {
            var tasks = _taskService.GetAllTasks(status, priority, category, search);
            return Ok(tasks);
        }

        /// <summary>
        /// Get task details by ID
        /// </summary>
        [HttpGet("{id:int}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound(new { message = $"Task with ID {id} was not found." });
            }
            return Ok(task);
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = _taskService.CreateTask(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        [HttpPut("{id:int}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = _taskService.UpdateTask(id, task);
            if (updated == null)
            {
                return NotFound(new { message = $"Task with ID {id} was not found." });
            }

            return Ok(updated);
        }

        /// <summary>
        /// Delete a task by ID
        /// </summary>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteTask(int id)
        {
            var success = _taskService.DeleteTask(id);
            if (!success)
            {
                return NotFound(new { message = $"Task with ID {id} was not found." });
            }

            return Ok(new { message = $"Task {id} deleted successfully." });
        }

        /// <summary>
        /// Toggle completed status of a task
        /// </summary>
        [HttpPatch("{id:int}/toggle")]
        public IActionResult ToggleStatus(int id)
        {
            var task = _taskService.ToggleTaskStatus(id);
            if (task == null)
            {
                return NotFound(new { message = $"Task with ID {id} was not found." });
            }

            return Ok(task);
        }

        /// <summary>
        /// Get real-time dashboard analytics & counters
        /// </summary>
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stats = _taskService.GetDashboardStats();
            return Ok(stats);
        }
    }
}
