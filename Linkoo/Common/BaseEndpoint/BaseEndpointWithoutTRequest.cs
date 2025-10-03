
using ReportApp.Common.views;
using MediatR;

namespace ReportApp.Common.BaseEndPoint
{
    public class BaseEndpointWithoutTRequest
    {
        protected IMediator _mediator;
        protected UserInfo _userInfo;

        public BaseEndpointWithoutTRequest(BaseEndpointParamterWithoutTRquest parameters)
        {
            _mediator = parameters.Mediator;
            _userInfo = parameters.UserInfo;
        }
    }
}
