using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.views;
using ReportApp.Domain.Repository;
using ReportApp.Model;

namespace ReportApp.Features.ReportManagment.GetAllNotifactionToScpecifcUser.Query
{
    public record GetUserNotificationsQuery()
    : IRequest<RequestResult<List<NotificationViewModel>>>;
    public class GetUserNotificationsQueryHandler
    : BaseRequestHandler<GetUserNotificationsQuery, RequestResult<List<NotificationViewModel>>, Notification>
    {
        private readonly IRepository<Notification> _notificationRepository;

        public GetUserNotificationsQueryHandler(BaseRequestHandlerParamters<Notification> parameters)
            : base(parameters)
        {
            _notificationRepository = parameters.Repository;
        }

        public override async Task<RequestResult<List<NotificationViewModel>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var user = _userInfo.UserId;
            if (user == null || user == "-1")
                return RequestResult<List<NotificationViewModel>>.Failure(ErrorCode.UserNotFound, "User not verified");

            var notifications = await _notificationRepository
                .Get(n => n.AppUserId == Guid.Parse(user))
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationViewModel
                {
                    Id = n.Id,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead
                })
                .ToListAsync(cancellationToken);

            return RequestResult<List<NotificationViewModel>>.Success(notifications, "Notifications retrieved successfully");
        }
    }


}
