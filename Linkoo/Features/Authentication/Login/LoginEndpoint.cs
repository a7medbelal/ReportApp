
using Reportapp.Features.Authentication.Login;
using Reportapp.Features.Authentication.Login.Command;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using Microsoft.AspNetCore.Mvc;

namespace ReportApp.Features.Authentication.Login
{
    [Route("[controller]/[action]")]
    public class LoginEndpoint : BaseEndpoint<RequestLoginViewModel, LoginDTO>
    {
        public LoginEndpoint(BaseEndpointParamter<RequestLoginViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<LoginDTO>> LogInUser(RequestLoginViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var loginCommand = new LoginUserCommand(viewmodel.Email, viewmodel.Password);
            
            var logInToken = await _mediator.Send(loginCommand);

            return logInToken.ToEndpointResponse(data => data);
        }
    }
}
