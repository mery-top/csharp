using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskPulse.Models;
using TaskPulse.Services;

namespace TaskPulse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ITaskService taskService, ILogger<HomeController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public IActionResult Index(string? status, string? priority, string? category, string? search)
        {
            var tasks = _taskService.GetAllTasks(status, priority, category, search);
            var stats = _taskService.GetDashboardStats();
            var categories = _taskService.GetCategories();

            ViewBag.Stats = stats;
            ViewBag.Categories = categories;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPriority = priority;
            ViewBag.CurrentCategory = category;
            ViewBag.SearchTerm = search;

            return View(tasks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
