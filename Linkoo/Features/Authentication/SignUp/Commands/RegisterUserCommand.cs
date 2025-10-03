
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Features.Authentication.SignUp.Commands;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Model;
using ReportApp.Model.Enum;
using System.Runtime.InteropServices;

namespace Reportapp.Features.Authentication.SignUp.Commands
{

    public record RegisterUserCommand( string Name, string Email, string Password , Role role , IFormFile? PersidentSignature = null ) : IRequest<RequestResult<UserViewModel>>;

    public class RegisterUserCommandHandler : BaseRequestHandler<RegisterUserCommand,RequestResult<UserViewModel>, AppUser>
    {
        readonly IPasswordHelper passwordHelper;
        public RegisterUserCommandHandler(BaseRequestHandlerParamters <AppUser> parameters, IPasswordHelper passwordHelper ) : base(parameters)
        {
            this.passwordHelper = passwordHelper;   
        }

        public async override Task<RequestResult<UserViewModel>>Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        { 
           var existingUser = await _repository.AnyAsync(u => u.Email == request.Email);
            
            if (existingUser)
            {
                return RequestResult<UserViewModel>.Failure(ErrorCode.alreadyExist, "Email is already in use.");
            }
            var hashedPassword = passwordHelper.createPasswordHash(request.Password);

             string? filePath = null;
            if (request.role == Role.President)
            {
                    // Optional: validate file size and type
                    const long maxSize = 10 * 1024 * 1024; // 10MB
                    if (request.PersidentSignature.Length > maxSize)
                        return RequestResult<UserViewModel>.Failure(ReportApp.Common.Enum.ErrorCode.FileSizeIsBig, "File size exceeds the limit of 10MB.");

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileExtension = Path.GetExtension(request.PersidentSignature.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await request.PersidentSignature.CopyToAsync(stream);
                    filePath = $"/uploads/{uniqueFileName}";
                

            }


            var newUser = new AppUser
            {
                Fname = request.Name,
                Email = request.Email,
                HashedPassword = hashedPassword.hash,
                role = request.role,
                SaltPassword = hashedPassword.salt,
                CreatedAt = DateTime.UtcNow
                ,Createdby = _userInfo.UserId,
                 PersidentSignature = filePath
            };
           


            await _repository.AddAsync(newUser);
            await _repository.SaveChangesAsync();
            return RequestResult<UserViewModel>.Success(new UserViewModel
            {
                 Id = newUser.Id,
                 email = newUser.Email,
                 Password = request.Password,
                 role = request.role.ToString(),
                 Signature =newUser.PersidentSignature != null ? $"https://reportapp.runasp.net/{newUser.PersidentSignature}":" "

            }, "Account CreateSuccfully");
        }
    }

}