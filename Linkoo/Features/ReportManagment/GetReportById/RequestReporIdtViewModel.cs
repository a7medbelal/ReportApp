using FluentValidation;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.GetReportById
{
    public record RequestReporIdtViewModel(Guid Id ) ;

    public class RequestReportViewModelValidator : AbstractValidator<RequestReporIdtViewModel>
    {
        public RequestReportViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");   
        }
    }
}
