using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Helper;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Domain.Repository;
using ReportApp.Features.Common.Notification;
using ReportApp.Model;

namespace ReportApp.Features.ReportManagment.CreateReport.Command
{
    public record CreateReportCommand(string Title, string Describtion ,string Subject ,IFormFile? Attachment ) : IRequest<RequestResult<CreateReportDTO>>;

    public class CreateReportCommandHandler : BaseRequestHandler<CreateReportCommand, RequestResult<CreateReportDTO>, Report>
    {
       readonly IRepository<AppUser> _userRepository;

        public CreateReportCommandHandler(BaseRequestHandlerParamters<Report> paramters, IPasswordHelper passwordHelper , IRepository<AppUser> userRepository) : base(paramters)
        {
            _userRepository = userRepository;


        }

        public override async Task<RequestResult<CreateReportDTO>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
           var user = _userInfo.UserId;

            if (user == null || user == "-1")
                return RequestResult<CreateReportDTO>.Failure(ReportApp.Common.Enum.ErrorCode.UserNotFound, "user not Varifaid");

            var ExistReport = await _repository.AnyAsync(r => r.Title == request.Title); 
            if (ExistReport) {
                return RequestResult<CreateReportDTO>.Failure(ReportApp.Common.Enum.ErrorCode.DuplicateRecord, "Report with the same title already exists.");
            }
            string? filePath = null;

            if (request.Attachment is not null )
            {
                // Optional: validate file size and type
                const long maxSize = 10 * 1024 * 1024; // 10MB
                if (request.Attachment.Length > maxSize)
                    return RequestResult<CreateReportDTO>.Failure(ReportApp.Common.Enum.ErrorCode.FileSizeIsBig, "File size exceeds the limit of 10MB.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileExtension = Path.GetExtension(request.Attachment.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Attachment.CopyToAsync(stream);
                filePath = $"/uploads/{uniqueFileName}";
            }


            var newReport = new Report
            {
                Title = request.Title,
                Description = request.Describtion,
                Subject = request.Subject,
                CreatedById = Guid.Parse(user),
                Createdby = user ,
                FilePath = filePath,    
            };
            await _repository.AddAsync(newReport);
            await _repository.SaveChangesAsync();
            var presidentId = await _userRepository.Get(u => u.role == Model.Enum.Role.President) // أو أي شرط مناسب
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);


            if (presidentId != Guid.Empty)
            {
                var message = $"تم إنشاء قرار جديد بعنوان {newReport.Title} من {_userInfo.UserName}";

                await _mediator.Publish(new UserNotificationEvent(
                    presidentId,
                    message,
                    DateTime.Now,newReport.Id));
            }


            return RequestResult<CreateReportDTO>.Success(new CreateReportDTO { ID = newReport.Id , Title = newReport.Title ,Description = newReport.Description , Subject = newReport.Subject}, "Report created successfully");  
        }
    }
}
