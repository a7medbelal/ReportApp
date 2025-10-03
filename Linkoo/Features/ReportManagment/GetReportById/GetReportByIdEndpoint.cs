using Azure;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Common.Views;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Features.ReportManagment.CreateReport.VMs;
using ReportApp.Features.ReportManagment.GetAllReport;
using ReportApp.Features.ReportManagment.GetAllReport.Query;
using ReportApp.Features.ReportManagment.GetReportById.Query;
using static Google.Apis.Requests.BatchRequest;

namespace ReportApp.Features.ReportManagment.GetReportById
{
    public class GetReportByIdEndpoint : BaseEndpoint<RequestReporIdtViewModel, GetReportByIdResponseViewMdoel>
    {
        public GetReportByIdEndpoint(BaseEndpointParamter<RequestReporIdtViewModel> parameters) : base(parameters)
        {
        }

        [HttpGet("Report/Get/id")]
        public async Task<EndpointResponse<GetReportByIdResponseViewMdoel>> CreateReport([FromQuery] RequestReporIdtViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var request = new GetReportByIdQuery(viewmodel.Id);

            var result = await _mediator.Send(request);


            return result.ToEndpointResponse(data => new GetReportByIdResponseViewMdoel
            {
                Name = data.OwnerName,
                Title = data.Title,
                Description = data.Description,
                Subject = data.Subject,
                CreatedAt = data.CreatedAt,
                ApprovedAt = data.ApprovedAt,
                Status = data.Status
               , Num = data.Num,
                filePath = data.filePath
            });
        }
    }
}
