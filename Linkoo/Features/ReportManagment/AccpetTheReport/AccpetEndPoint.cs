using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.AccpetTheReport.Command;
using ReportApp.Filters;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.AccpetTheReport
{
    public class ApproveOrRejectReportEndpoint
     : BaseEndpoint<RequestApproveReportViewModel, ResponseApproveReportViewModel>
    {
        public ApproveOrRejectReportEndpoint(BaseEndpointParamter<RequestApproveReportViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost("Report/ApproveOrReject")]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.ApproveReport })]
        [Authorize]
        public async Task<EndpointResponse<ResponseApproveReportViewModel>> ApproveOrReject([FromBody] RequestApproveReportViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;

            var command = new AccpetOrRejectReportCommand(viewModel.Id, viewModel.IsApproved);
            var result = await _mediator.Send(command);

            return result.ToEndpointResponse(data => data);
        }
    }

}
