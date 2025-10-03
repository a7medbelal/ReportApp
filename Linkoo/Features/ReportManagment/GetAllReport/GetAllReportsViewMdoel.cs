using ReportApp.Common.Views;

namespace ReportApp.Features.ReportManagment.GetAllReport
{
    public class GetAllReportsViewMdoel
    {
        public PagingViewModel<GetAllReportsResponse> pagingViewModel { get; set; }
  
    }

    public class GetAllReportsResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Status { get; set; }

    }
}
