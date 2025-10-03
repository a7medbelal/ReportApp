using FluentValidation;
using Reportapp.Features.Authentication.Login;

namespace ReportApp.Features.ReportManagment.DeleteReport
{
    public class RequestDeleteReportViewModel
    {
        public Guid Id { get; set; }

    }

    public class RequestDeleteReportViewModelValidator : AbstractValidator<RequestDeleteReportViewModel>
    {
        public RequestDeleteReportViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Report Id is required");

        }
    }
}
