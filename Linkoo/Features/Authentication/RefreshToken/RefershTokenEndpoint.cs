using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.Authentication.RefreshToken.Command;

namespace ReportApp.Features.Authentication.RefreshToken
{
    public class RefreshTokenEndpoint
        : BaseEndpoint<RequestRefreshTokenViewModel, RefreshTokenResponseViewModel>
    {
        public RefreshTokenEndpoint(BaseEndpointParamter<RequestRefreshTokenViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost("Auth/Refresh")]
        [AllowAnonymous] 
        public async Task<EndpointResponse<RefreshTokenResponseViewModel>> Refresh([FromBody] RequestRefreshTokenViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;

            var command = new RefreshTokenCommand(viewModel.RefreshToken);
            var result = await _mediator.Send(command);

            return result.ToEndpointResponse(data => data);
        }
    }

}
