namespace ReportApp.Features.ReportManagment.GetAllNotifactionToScpecifcUser
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}