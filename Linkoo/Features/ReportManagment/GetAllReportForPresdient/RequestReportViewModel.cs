using FluentValidation;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.GetAllReportForPresdient
{
    public record RequestReportViewModel(int pageSize = 10, int pageIndex = 1,string? Keword = null , ReportStatus? Status = null ,DateTime? CreatedFrom =null , DateTime? CreatedTo = null ) ;

    public class RequestReportViewModelValidator : AbstractValidator<RequestReportViewModel>
    {
        public RequestReportViewModelValidator()
        {
            RuleFor(x => x.pageSize).GreaterThan(0).WithMessage("Page size must be greater than 0.");
            RuleFor(x => x.pageIndex).GreaterThan(0).WithMessage("Page index must be greater than 0.");
            RuleFor(x => x.Keword).MaximumLength(100).WithMessage(" name cannot exceed 100 characters.");
        }
    }
}
