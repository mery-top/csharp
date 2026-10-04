namespace TaskPulse.Models
{
    public class DashboardStats
    {
        public int TotalTasks { get; set; }
        public int TodoTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int UrgentTasks { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
