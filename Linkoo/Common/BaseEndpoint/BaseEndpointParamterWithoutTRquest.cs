
using ReportApp.Common.views;
using MediatR;

namespace ReportApp.Common.BaseEndPoint
{
    public class BaseEndpointParamterWithoutTRquest
    {
        protected IMediator _mediator;
        protected UserInfo _userInfo;

     
        public IMediator Mediator => _mediator;
        public UserInfo UserInfo => _userInfo;

        public BaseEndpointParamterWithoutTRquest(IMediator mediator, UserInfoProvider userInfoProvider)
        {
            _mediator = mediator;
            _userInfo = userInfoProvider.UserInfo;
        }



    }
}
