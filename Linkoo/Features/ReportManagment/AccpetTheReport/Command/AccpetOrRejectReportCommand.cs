using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.views;
using ReportApp.Domain;
using ReportApp.Domain.Repository;
using ReportApp.Features.Common.Notification;
using ReportApp.Model;

namespace ReportApp.Features.ReportManagment.AccpetTheReport.Command
{
    public record AccpetOrRejectReportCommand(Guid ReportId,
    bool IsApproved) : IRequest<RequestResult<ResponseApproveReportViewModel>>;

    public class ApproveOrRejectReportCommandHandler : BaseRequestHandler<AccpetOrRejectReportCommand, RequestResult<ResponseApproveReportViewModel>, Report>
    {
        private readonly Context _context;


        public ApproveOrRejectReportCommandHandler(BaseRequestHandlerParamters<Report> parameters , Context context)
            : base(parameters)
        {
            _context = context;

        }

        public override async Task<RequestResult<ResponseApproveReportViewModel>> Handle(AccpetOrRejectReportCommand request, CancellationToken cancellationToken)
        {
            var user = _userInfo.UserId;
            if (user == null || user == "-1")
                return RequestResult<ResponseApproveReportViewModel>.Failure(ErrorCode.UserNotFound, "User not verified");

            var report = await _repository.GetByIdAsync(request.ReportId);
            if (report == null)
                return RequestResult<ResponseApproveReportViewModel>.Failure(ErrorCode.NotFound, "Report not found");

            if (report.Status != Model.Enum.ReportStatus.Pending)
                return RequestResult<ResponseApproveReportViewModel>.Failure(ErrorCode.InvalidOperation, "Only pending reports can be approved or rejected");

            Report updateReport;    
            if (request.IsApproved)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // الحصول على الرقم التسلسلي التالي من الـ Sequence باستخدام ExecuteScalar
                    var command = _context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "SELECT NEXT VALUE FOR ReportNumbers";
                    command.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
                    await _context.Database.OpenConnectionAsync(cancellationToken);
                    var nextNumber = (long)await command.ExecuteScalarAsync(cancellationToken);

                    updateReport = new Report
                    {
                        Id = report.Id,
                        CreatedAt = report.CreatedAt,
                        Status = Model.Enum.ReportStatus.Approved,
                        UpdatedBy = user,
                        ApprovedAt = DateTime.Now,
                        Number = nextNumber
                    };


                    await _repository.UpdateAsync(updateReport, nameof(updateReport.ApprovedAt), nameof(updateReport.Number), nameof(updateReport.UpdatedBy), nameof(updateReport.Status));
                    await _repository.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return RequestResult<ResponseApproveReportViewModel>.Failure(ErrorCode.InternalError, "Failed to approve report");
                }
            
           }
            else
            {

                 updateReport = new Report
                  {
                    Id = report.Id,
                    Status = Model.Enum.ReportStatus.Rejected,
                    UpdatedBy = user,
                    ApprovedAt = DateTime.Now

                  };
                 await _repository.UpdateAsync(updateReport ,nameof(updateReport.Status),nameof(updateReport.ApprovedAt) , nameof(updateReport.UpdatedBy));
                    await _repository.SaveChangesAsync();

            }

            // action take here to delet the notification 
            await _mediator.Send(new DeleteNotificationThatHasActionCommand(request.ReportId)); 

            var message = request.IsApproved
                ?$"لقد تمت الموافقة على القرار بعنوان \"{report.Title}\" برقم {updateReport.Number}"
                 : $"تم رفض القرار الخاص بك بعنوان \"{report.Title}\"";
            

            await _mediator.Publish(new UserNotificationEvent(
                report.CreatedById,
                message,
                DateTime.UtcNow));


            return RequestResult<ResponseApproveReportViewModel>.Success(
                new ResponseApproveReportViewModel
                {
                    ReportId  = report.Id,
                    Title = report.Title,
                    Status = updateReport.Status.ToString(),
                    ApprovedAt = updateReport.ApprovedAt,
                    Number = updateReport.Number,
                },
                request.IsApproved ? "Report approved successfully" : "Report rejected successfully"
            );
        }
    }
}

