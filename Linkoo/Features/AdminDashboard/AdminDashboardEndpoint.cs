using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.GetAllReport.Query;
using ReportApp.Filters;
using ReportApp.Model.Enum;

namespace ReportApp.Features.AdminDashboard
{
    public class AdminDashboardEndpoint : BaseEndpoint< Unit, DashbordViewModel>
    {
        public AdminDashboardEndpoint(BaseEndpointParamter<Unit> parameters) : base(parameters)
        {
        }

        [HttpGet("Admin/Dashboard")]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.AdminDashboard })]
        [Authorize]
        public async Task<EndpointResponse<DashbordViewModel>> AdminDashBoard()
        {
            var request = new AdminDashboardQuery();

            var result = await _mediator.Send(request);

            return result.ToEndpointResponse(data => result.data);
        }
    }
}
