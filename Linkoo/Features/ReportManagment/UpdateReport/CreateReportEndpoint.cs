using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reportapp.Features.Authentication.Login;
using Reportapp.Features.Authentication.Login.Command;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Features.ReportManagment.CreateReport.VMs;
using ReportApp.Features.ReportManagment.UpdateReport;
using ReportApp.Filters;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.CreateReport
{
    public class UpdateReportEndpoint : BaseEndpoint<RequestUpdateReportViewModel, ResponseUpdateReportViewModel>
    {
        public UpdateReportEndpoint(BaseEndpointParamter<RequestUpdateReportViewModel> parameters) : base(parameters)
        {
        }

        [HttpPut("Report/Update")]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.UpdateReport })]
        [Authorize]
        public async Task<EndpointResponse<ResponseUpdateReportViewModel>> UpdateReport([FromForm] RequestUpdateReportViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var request = new UpdateReportCommand(viewmodel.Id,viewmodel.Title, viewmodel.Description, viewmodel.Subject, viewmodel.File);

            var result = await _mediator.Send(request);


            return result.ToEndpointResponse(data => data);
        }
    }
}