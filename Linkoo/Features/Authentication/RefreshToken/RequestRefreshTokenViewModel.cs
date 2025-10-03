using FluentValidation;

namespace ReportApp.Features.Authentication.RefreshToken
{
    public class RequestRefreshTokenViewModel
    {
        public string RefreshToken { get; set; }
    }

    public class RequestRefreshTokenViewModelValidator : AbstractValidator<RequestRefreshTokenViewModel>
    {
        public RequestRefreshTokenViewModelValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
        }
    }
}