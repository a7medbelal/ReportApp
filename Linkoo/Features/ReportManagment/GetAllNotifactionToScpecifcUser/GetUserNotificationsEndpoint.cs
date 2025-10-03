using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.GetAllNotifactionToScpecifcUser.Query;
using ReportApp.Filters;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.GetAllNotifactionToScpecifcUser
{
    public class GetUserNotificationsEndpoint : BaseEndpoint<Unit, List<NotificationViewModel>>
    {
        public GetUserNotificationsEndpoint(BaseEndpointParamter<Unit> parameters) : base(parameters)
        {
        }

        [HttpGet("Notifications/GetAll")]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.ViewNotifications })]
        [Authorize]
        public async Task<EndpointResponse<List<NotificationViewModel>>> GetUserNotifications()
        {
            var result = await _mediator.Send(new GetUserNotificationsQuery());
            return result.ToEndpointResponse(data => data);
        }
    }
   
}
