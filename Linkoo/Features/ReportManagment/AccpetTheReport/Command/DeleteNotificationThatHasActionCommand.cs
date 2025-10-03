using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Model;

namespace ReportApp.Features.ReportManagment.AccpetTheReport.Command
{


    public record DeleteNotificationThatHasActionCommand(Guid id) : IRequest<RequestResult<bool>>;

    public class DeleteNotificationThatHasActionCommandHandler : BaseRequestHandler<DeleteNotificationThatHasActionCommand, RequestResult<bool>,Notification >
    {


        public DeleteNotificationThatHasActionCommandHandler(BaseRequestHandlerParamters<Notification> paramters, IPasswordHelper passwordHelper) : base(paramters)
        {

        }

        public override async Task<RequestResult<bool>> Handle(DeleteNotificationThatHasActionCommand request, CancellationToken cancellationToken)
        {
            var user = _userInfo.UserId;

            if (user == null || user == "-1")
                return RequestResult<bool>.Failure(ReportApp.Common.Enum.ErrorCode.UserNotFound, "user not Varifaid");

            var ExistReport = await _repository.Get(c => c.ReportId == request.id).Select(c=>c.Id).FirstOrDefaultAsync();

            if (ExistReport == null )
            {
                return RequestResult<bool>.Failure(ReportApp.Common.Enum.ErrorCode.NotFound, "No Notificaiton found .");
            }

            var DeletedNotificaiton = new Notification
            {
                Id = ExistReport,
                IsDeleted = true
            };
            await _repository.UpdateAsync(DeletedNotificaiton, nameof(DeletedNotificaiton.IsDeleted));

            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Notifaction Deleted successfully");
        }
    }
}