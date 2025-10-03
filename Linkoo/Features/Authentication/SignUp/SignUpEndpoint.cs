using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Reportapp.Features.Authentication.SignUp.Commands;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.Authentication.SignUp.Commands;
using ReportApp.Filters;
using ReportApp.Model.Enum;


namespace ReportApp.Features.Authentication.SignUp;
[Route("[controller]/[action]")]
public class SignUpEndpoint : BaseEndpoint<SignUpRequestViewModel, UserViewModel>
{
    public SignUpEndpoint(BaseEndpointParamter<SignUpRequestViewModel> parameters) : base(parameters)
    {
    }

    [HttpPost]
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.createUser })]
    public async Task<EndpointResponse<UserViewModel>> CreateUser([FromForm]SignUpRequestViewModel request)
    {
        // Validate the inputs 
        var validationResponse = ValidateRequest(request);
        if (!validationResponse.isSuccess)
            return validationResponse;

        var command = new RegisterUserCommand(request.Name, request.Email, request.Password ,request.Role , request.Signature);

        var result = await _mediator.Send(command);
        if (!result.isSuccess)
            return EndpointResponse<UserViewModel>.Failure(result.errorCode, result.message);

        return EndpointResponse<UserViewModel>.Success(result.data, result.message);

    }

}