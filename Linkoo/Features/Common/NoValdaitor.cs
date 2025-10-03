using FluentValidation;

namespace ReportApp.Features.Common
{
    public class NoOpValidator<T> : AbstractValidator<T>
    {
        public NoOpValidator() { }
    }
}
