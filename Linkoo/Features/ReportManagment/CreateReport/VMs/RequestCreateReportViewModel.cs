using FluentValidation;
using Reportapp.Features.Authentication.Login;

namespace ReportApp.Features.ReportManagment.CreateReport.VMs
{
    public class RequestCreateReportViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public IFormFile? File
        {
            get; set;
        }
    }

    public class RequestCreateReportViewModelValidator : AbstractValidator<RequestCreateReportViewModel>
    {
        public RequestCreateReportViewModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required");
        }
    }
}
