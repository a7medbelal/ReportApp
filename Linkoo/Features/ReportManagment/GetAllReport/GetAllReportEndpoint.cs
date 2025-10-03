using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Common.Views;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Features.ReportManagment.CreateReport.VMs;
using ReportApp.Features.ReportManagment.GetAllReport.Query;
using ReportApp.Filters;
using ReportApp.Model.Enum;
using static Google.Apis.Requests.BatchRequest;

namespace ReportApp.Features.ReportManagment.GetAllReport
{
    [Route("[controller]/[action]")]
    public class GetAllReportEndpoint : BaseEndpoint<RequestReportViewModel , GetAllReportsViewMdoel>
    {
        public GetAllReportEndpoint(BaseEndpointParamter<RequestReportViewModel> parameters) : base(parameters)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.GetAllReport})]
        [Authorize]
        public async Task<EndpointResponse<GetAllReportsViewMdoel>> CreateReport([FromQuery] RequestReportViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var request = new GetAllReportsQuery(viewmodel.pageSize, viewmodel.pageIndex, viewmodel.Keword, viewmodel.Status, viewmodel.CreatedFrom, viewmodel.CreatedTo);

            var result = await _mediator.Send(request);
            var requestItems = result.data.Items.Select(c => new GetAllReportsResponse
            {
                Id = c.Id,
                Title = c.Title,
                CreatedAt = c.CreatedAt
              , ApprovedAt = c.ApprovedAt,
                Status = c.Status
            }).ToList();


            return result.ToEndpointResponse(data => new GetAllReportsViewMdoel
            {
                pagingViewModel = new PagingViewModel<GetAllReportsResponse>
                {
                    Items = requestItems,
                    PageIndex = result.data.PageIndex,
                    Records = result.data.Records,
                    PageSize = result.data.PageSize

                }
            });
        }
    }
}
