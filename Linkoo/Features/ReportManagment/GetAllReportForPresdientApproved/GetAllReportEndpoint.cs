using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Common.Views;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Features.ReportManagment.CreateReport.VMs;
using ReportApp.Features.ReportManagment.GetAllReport.Query;
using ReportApp.Features.ReportManagment.GetAllReportForPresdient.Query;
using ReportApp.Features.ReportManagment.GetAllReportForPresdientApproved.Query;
using ReportApp.Filters;
using ReportApp.Model.Enum;
using static Google.Apis.Requests.BatchRequest;

namespace ReportApp.Features.ReportManagment.GetAllReportForPresdientApproved
{
    public class GetAllReportEndpoint : BaseEndpoint<RequestReportViewModel , PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>>
    {
        public GetAllReportEndpoint(BaseEndpointParamter<RequestReportViewModel> parameters) : base(parameters)
        {
        }

        [HttpGet("Report/GetAllForPresidentApproved")]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.GetAllReportsForPresident })]
        [Authorize] 
        public async Task<EndpointResponse<PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>>> GetAllForPersdentApproved([FromQuery] RequestReportViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var request = new GetAllReportForPresdientApprovedQuery(viewmodel.pageSize, viewmodel.pageIndex, viewmodel.Keword, viewmodel.Status, viewmodel.CreatedFrom, viewmodel.CreatedTo);

            var result = await _mediator.Send(request);

            return result.ToEndpointResponse(data => result.data);
        }
    }
}
