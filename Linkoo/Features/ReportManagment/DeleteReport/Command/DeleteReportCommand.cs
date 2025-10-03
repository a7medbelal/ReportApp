using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Helper;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Model;

namespace ReportApp.Features.ReportManagment.DeleteReport.Command
{
    public record DeleteReportCommand(Guid id ) : IRequest<RequestResult<bool>>;

    public class DeleteReportCommandHandler : BaseRequestHandler<DeleteReportCommand, RequestResult<bool>, Report>
    {
       

        public DeleteReportCommandHandler(BaseRequestHandlerParamters<Report> paramters, IPasswordHelper passwordHelper) : base(paramters)
        {
         
        }

        public override async Task<RequestResult<bool>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
           var user = _userInfo.UserId;

            if (user == null || user == "-1")
                return RequestResult<bool>.Failure(ReportApp.Common.Enum.ErrorCode.UserNotFound, "user not Varifaid");

            var ExistReport = await _repository.Get(c => c.Id == request.id && c.Status == Model.Enum.ReportStatus.Pending).Select(c=> new {c.Status , c.Id}).FirstOrDefaultAsync();

            if (ExistReport is null ) {
                return RequestResult<bool>.Failure(ReportApp.Common.Enum.ErrorCode.NoReportFound, "No Report found .");
            }
            if (ExistReport.Status != Model.Enum.ReportStatus.Pending)
            {
                return RequestResult<bool>.Failure(ReportApp.Common.Enum.ErrorCode.InvalidOperation, "Only reports with 'Pending' status can be deleted.");
            }

            var DeletedReport = new Report { 
             Id = request.id,   
                IsDeleted = true
            };
            await _repository.UpdateAsync(DeletedReport ,nameof(DeletedReport.IsDeleted));

            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Report Deleted successfully");  
        }
    }
}
