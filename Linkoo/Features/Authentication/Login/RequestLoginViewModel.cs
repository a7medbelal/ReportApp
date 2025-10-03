using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Reportapp.Features.Authentication.Login
{
    public record RequestLoginViewModel(string Email , string Password);

    public class RequestLoginViewModelValidator : AbstractValidator<RequestLoginViewModel>
    {
        public RequestLoginViewModelValidator()
        {
        }
    }
}