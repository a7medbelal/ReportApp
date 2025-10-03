using FluentValidation;

namespace ReportApp.Features.ReportManagment.AccpetTheReport
{

        public class RequestApproveReportViewModel
        {
            public Guid Id { get; set; }
            public bool IsApproved { get; set; }
        }

        public class RequestApproveReportViewModelValidator : AbstractValidator<RequestApproveReportViewModel>
        {
            public RequestApproveReportViewModelValidator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Report Id is required");
                RuleFor(x => x.IsApproved).NotNull().WithMessage("Approval decision is required");
            }
        }
    }