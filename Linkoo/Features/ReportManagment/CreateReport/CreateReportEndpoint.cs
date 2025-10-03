using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reportapp.Features.Authentication.Login;
using Reportapp.Features.Authentication.Login.Command;
using Reportapp.Features.Authentication.Login.Login;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.views;
using ReportApp.Features.ReportManagment.CreateReport.Command;
using ReportApp.Features.ReportManagment.CreateReport.VMs;
using ReportApp.Filters;
using ReportApp.Model.Enum;

namespace ReportApp.Features.ReportManagment.CreateReport
{
    [Route("[controller]/[action]")]
    public class CreateReportEndpoint : BaseEndpoint<RequestCreateReportViewModel, ResponseCreateReportViewModel>
    {
        public CreateReportEndpoint(BaseEndpointParamter<RequestCreateReportViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { AppFeature.CreateReport })]
        [Authorize]
        public async Task<EndpointResponse<ResponseCreateReportViewModel>> CreateReport([FromForm] RequestCreateReportViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var request = new CreateReportCommand(viewmodel.Title, viewmodel.Description, viewmodel.Subject, viewmodel.File);

            var result = await _mediator.Send(request);


            return result.ToEndpointResponse(data => new ResponseCreateReportViewModel
            {
                Id = data.ID,
                Title = data.Title,
                Description = data.Description,
                Subject = data.Subject,
            });
        }
    }
}