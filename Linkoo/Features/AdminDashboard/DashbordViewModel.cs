namespace ReportApp.Features.AdminDashboard
{
    public class DashbordViewModel
    {
        public int UsersCount { get; set; }
        public int ReportsCount { get; set; }
        public int CancelledReportsCount { get; set; }
        public int InProgressReportsCount { get; set; } 
        public int ApprovedReportsCount { get; set; }
    }
}
