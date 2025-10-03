using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reportapp.Features.Authentication.Login;
using Reportapp.Features.Authentication.Login.Command;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Features.ReportManagment.CreateReport.VMs;
using ReportApp.Features.ReportManagment.DeleteReport.Command;
using ReportApp.Filters;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.DeleteReport
{
    public class DeleteReportEndpoint : BaseEndpoint<RequestDeleteReportViewModel, bool>
    {
        public DeleteReportEndpoint(BaseEndpointParamter<RequestDeleteReportViewModel> parameters) : base(parameters)
        {
        }

        [HttpDelete("Report/Delete")]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.DeleteReport })]
        [Authorize]

        public async Task<EndpointResponse<bool>> DeleteReport([FromBody] RequestDeleteReportViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var request = new DeleteReportCommand(viewmodel.Id);

            var result = await _mediator.Send(request);


            return result.ToEndpointResponse(data => result.data); 
        }
    }
}