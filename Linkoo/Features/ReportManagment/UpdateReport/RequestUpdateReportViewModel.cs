using FluentValidation;
using Reportapp.Features.Authentication.Login;

namespace ReportApp.Features.ReportManagment.UpdateReport
{
    public class RequestUpdateReportViewModel
    {
        public Guid Id { get; set; }    
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public IFormFile? File
        {
            get; set;
        }
    }

    public class RequestUpdateReportViewModelValidator : AbstractValidator<RequestUpdateReportViewModel>
    {
        public RequestUpdateReportViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required");
        }
    }
}
