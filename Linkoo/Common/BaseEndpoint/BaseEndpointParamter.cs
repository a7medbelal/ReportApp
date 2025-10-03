using FluentValidation;
using ReportApp.Common.views;
using MediatR;

namespace ReportApp.Common.BaseEndpoint
{
    public class BaseEndpointParamter<TRequest>
    {
        readonly IMediator _mediator;
        readonly IValidator<TRequest> _validator;
        readonly UserInfo _userInfo;

        public IMediator Mediator => _mediator;
        public IValidator<TRequest> Validator => _validator;
        public UserInfo UserInfo => _userInfo;

        public BaseEndpointParamter(IMediator mediator, IValidator<TRequest> validator, UserInfoProvider userInfoProvider)
        {
            _mediator = mediator;
            _validator = validator;
            _userInfo = userInfoProvider.UserInfo;

        }
    }
}