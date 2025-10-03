
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Reportapp.Common.BaseHandler;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Model;

namespace Reportapp.Features.Authentication.Login.Command
{
    public record LoginUserCommand(string? Email, string? Password ) : IRequest<RequestResult<LoginDTO>>;

    public class LoginCommandHandler : BaseRequestHandler<LoginUserCommand, RequestResult<LoginDTO>, AppUser>
    {
        private readonly IPasswordHelper _passwordHelper;

        public LoginCommandHandler(BaseRequestHandlerParamters<AppUser> paramters, IPasswordHelper passwordHelper) : base(paramters)
        {
            _passwordHelper = passwordHelper;
        }

        public override async Task<RequestResult<LoginDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository
                    .Get(u => u.Email == request.Email)
                     .FirstOrDefaultAsync();
            if (user == null)
                return RequestResult<LoginDTO>.Failure(ErrorCode.InvalidData, "User not found");

            var CheckPassword = _passwordHelper.verifyPasswordHash(
             request.Password,
             user.HashedPassword,
             user.SaltPassword);

            if (!CheckPassword)
                return RequestResult<LoginDTO>.Failure(ErrorCode.InvalidData, "Invalid password");

            var token = await _tokenHelper.GenerateToken(user.Id.ToString(), user.role, user.Fname);

           var refreshToken =await _tokenHelper.GenerateRefreshToken();
            user.refreshToken.Add(refreshToken);
            await _repository.UpdateAsync(user , nameof(user.refreshToken));
            await _repository.SaveChangesAsync();
        
        var loginDTO = new LoginDTO
            {
                 ID = user.Id,
                Token = token
                , name = user.Fname 
                , Role = user.role.ToString()
                , refreshToken =refreshToken.Token,
                 refreshTokenExpiryTime = refreshToken.ExpiresOn
            };

            return RequestResult<LoginDTO>.Success(loginDTO, "Login successful");
        }
    }
}