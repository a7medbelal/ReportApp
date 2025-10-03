using MediatR;
using ReportApp.Common.views;
using ReportApp.Domain.Repository;

namespace ReportApp.Features.Common.Notification
{
    public record UserNotificationEvent(
        Guid UserId,       
        string Message,
        DateTime CreatedAt,
        Guid? ReportId = null
    ) : INotification;

    public class UserNotificationEventHandler
    : INotificationHandler<UserNotificationEvent>
    {
        private readonly IRepository<Model.Notification> _notificationRepository;
        private readonly IMediator _mediator;
        readonly UserInfo _userInfo;

        public UserNotificationEventHandler(
            IRepository<Model.Notification> notificationRepository,
            IMediator mediator, UserInfoProvider userInfoProvider )
        {
            _notificationRepository = notificationRepository;
            _mediator = mediator;
            _userInfo = userInfoProvider.UserInfo;
        }

        public async Task Handle(UserNotificationEvent notification, CancellationToken cancellationToken)
        {
            // Save notification in DB
            var entity = new Model.Notification
            {
                AppUserId = notification.UserId,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                Createdby  = _userInfo.UserName ?? "System"
                , ReportId = notification.ReportId,
            };

            await _notificationRepository.AddAsync(entity);
            await _notificationRepository.SaveChangesAsync();
        }
    }

}
