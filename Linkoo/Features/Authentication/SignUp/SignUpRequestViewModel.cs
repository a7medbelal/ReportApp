using FluentValidation;
using ReportApp.Model.Enum;


namespace ReportApp.Features.Authentication.SignUp;

public record SignUpRequestViewModel( string Name, string Email, string Password  ,Role Role, IFormFile? Signature);

public class SignUpRequestViewModelValidator : AbstractValidator<SignUpRequestViewModel>
{
    public SignUpRequestViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Please enter a correctly formatted email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(email => !email.Contains("gamil.com")).WithMessage("Did you mean 'gmail.com'? Please check your email.");

		RuleFor(x => x.Name)
				.NotEmpty().WithMessage(" Name is required.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage(" name must contain only letters.");

        When(x => x.Role == Role.Agent, () =>
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage(" Name is required for agents.")
            .MinimumLength(3).WithMessage(" Name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage(" Name must not exceed 50 characters.");
        }

    );	
        When(x => x.Role == Role.President , () =>
        {
            RuleFor(x => x.Signature)
            .NotNull().WithMessage("Signature is required for presidents.");
        });
    }



}


