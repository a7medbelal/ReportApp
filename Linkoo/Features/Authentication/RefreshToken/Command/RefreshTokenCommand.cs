using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.Helper;
using ReportApp.Common.views;
using ReportApp.Domain.Repository;
using ReportApp.Model;
using System.Security.Cryptography;

namespace ReportApp.Features.Authentication.RefreshToken.Command
{
    public record RefreshTokenCommand(string RefreshToken)
    : IRequest<RequestResult<RefreshTokenResponseViewModel>>;

    public class RefreshTokenCommandHandler
    : BaseRequestHandler<RefreshTokenCommand, RequestResult<RefreshTokenResponseViewModel>, AppUser>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly TokenHelper _tokenHelper;

        public RefreshTokenCommandHandler(BaseRequestHandlerParamters<AppUser> parameters, TokenHelper tokenHelper)
            : base(parameters)
        {
            _userRepository = parameters.Repository;
            _tokenHelper = tokenHelper;
        }

        public override async Task<RequestResult<RefreshTokenResponseViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository
             .Get(u => u.refreshToken.Any(rt => rt.Token == request.RefreshToken))
             .FirstOrDefaultAsync(); 

            var RefreshToken = user?.refreshToken.FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (user == null || RefreshToken.IsActive)
                return RequestResult<RefreshTokenResponseViewModel>.Failure(ErrorCode.InvalidData, "Invalid or expired refresh token");

            // Generate new Access Token + Refresh Token
            var newAccessToken = await _tokenHelper.GenerateToken(user.Id.ToString(), user.role, user.Fname);
            var newRefreshToken =await _tokenHelper.GenerateRefreshToken();
            user.refreshToken.Add(newRefreshToken);
         
           // await _userRepository.UpdateAsync(user, nameof(user.refreshToken)); 
            
            await _userRepository.SaveChangesAsync();

            var response = new RefreshTokenResponseViewModel
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresOn = newRefreshToken.ExpiresOn
            };

            return RequestResult<RefreshTokenResponseViewModel>.Success(response, "Token refreshed successfully");
        }
    }


}
