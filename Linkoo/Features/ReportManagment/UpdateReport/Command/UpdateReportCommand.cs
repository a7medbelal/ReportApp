using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Helper;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.UpdateReport;
using ReportApp.Model;

namespace ReportApp.Features.ReportManagment.CreateReport.Command
{
    public record UpdateReportCommand(Guid Id, string Title, string Describtion ,string Subject ,IFormFile? Attachment ) : IRequest<RequestResult<ResponseUpdateReportViewModel>>;

    public class UpdateReportCommandHandler : BaseRequestHandler<UpdateReportCommand, RequestResult<ResponseUpdateReportViewModel>, Report>
    {
       

        public UpdateReportCommandHandler(BaseRequestHandlerParamters<Report> paramters, IPasswordHelper passwordHelper) : base(paramters)
        {
         
        }

        public override async Task<RequestResult<ResponseUpdateReportViewModel>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
           var user = _userInfo.UserId;

            if (user == null || user == "-1")
                return RequestResult<ResponseUpdateReportViewModel>.Failure(ReportApp.Common.Enum.ErrorCode.UserNotFound, "user not Varifaid");

            var ExistReport = await _repository.AnyAsync(r => r.Title == request.Title && r.Id != request.Id  ); 
            if (ExistReport) {
                return RequestResult<ResponseUpdateReportViewModel>.Failure(ReportApp.Common.Enum.ErrorCode.DuplicateRecord, "Report with the same title already exists.");
            }
            var checkReport = await _repository.AnyAsync(c => c.Id == request.Id && c.Status != Model.Enum.ReportStatus.Pending);

            if (checkReport)
                return RequestResult<ResponseUpdateReportViewModel>.Failure(ReportApp.Common.Enum.ErrorCode.InvalidOperation, "Only reports with 'Pending' status can be updated.");
            string? filePath = null;

            if (request.Attachment is not null )
            {
                // Optional: validate file size and type
                const long maxSize = 10 * 1024 * 1024; // 10MB
                if (request.Attachment.Length > maxSize)
                    return RequestResult<ResponseUpdateReportViewModel>.Failure(ReportApp.Common.Enum.ErrorCode.FileSizeIsBig, "File size exceeds the limit of 10MB.");

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
                Id = request.Id,
                Title = request.Title,
                Description = request.Describtion,
                Subject = request.Subject,
                UpdatedBy = user,
                FilePath = filePath, 
                UpdatedAt = DateTime.UtcNow 
            };

            await _repository.UpdateAsync(newReport , nameof(newReport.Title) ,nameof(newReport.Description ), nameof(newReport.Subject),nameof(newReport.UpdatedAt) ,nameof(newReport.FilePath) , nameof(newReport.UpdatedBy));
            await _repository.SaveChangesAsync();


            return RequestResult<ResponseUpdateReportViewModel>.Success(new ResponseUpdateReportViewModel { Id = newReport.Id , Title = newReport.Title ,Description = newReport.Description , Subject = newReport.Subject , FilePath =newReport.FilePath}, "Report updated successfully");  
        }
    }
}
