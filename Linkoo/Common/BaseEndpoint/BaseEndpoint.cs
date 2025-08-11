using FluentValidation;
using Linkoo.Common.Enum;
using Linkoo.Common.views;
using MediatR;

namespace Linkoo.Common.BaseEndpoint
{
    public class BaseEndpoint<TRequest , TResponse>
    {

        protected IMediator _mediator;
        protected IValidator<TRequest> _validator;
        protected UserInfo _userInfo;

        public BaseEndpoint(BaseEndpointParamter<TRequest> parameters)
        {
            _mediator = parameters.Mediator;
            _validator = parameters.Validator;
            _userInfo = parameters.UserInfo;
        }

        protected EndpointResponse<TResponse> ValidateRequest(TRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var validationError = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

                return EndpointResponse<TResponse>.Failure(ErrorCode.InvalidData, validationError);
            }

            return EndpointResponse<TResponse>.Success(default);
        }

    }
}
