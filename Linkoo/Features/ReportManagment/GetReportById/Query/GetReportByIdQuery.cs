using MediatR;
using Microsoft.EntityFrameworkCore;
using PredicateExtensions;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.Helpers;
using ReportApp.Common.views;
using ReportApp.Common.Views;
using ReportApp.Domain.Repository;
using ReportApp.Model;
using ReportApp.Model.Enum;
using System.Linq.Expressions;

namespace ReportApp.Features.ReportManagment.GetReportById.Query
{
    public record GetReportByIdQuery(Guid Id) : IRequest<RequestResult<GetReportsByidDTO>>;

    public class GetReportByIdQueryHandler : BaseRequestHandler<GetReportByIdQuery, RequestResult<GetReportsByidDTO> , Report>
    {


        public GetReportByIdQueryHandler(BaseRequestHandlerParamters<Report> paramters) : base(paramters)
        {
        }
        public override async Task<RequestResult<GetReportsByidDTO>> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        { 


            var requestReport =await _repository.Get(c=>c.Id == request.Id).
                Select(r => new GetReportsByidDTO
                {
                     OwnerName = r.CreatedBy.Fname,
                     Title = r.Title,
                     Description = r.Description,
                     Subject = r.Subject,
                     CreatedAt = r.CreatedAt,
                     ApprovedAt = r.ApprovedAt,
                     Status = r.Status.ToString(),
                     filePath =r.FilePath != null ? $"https://reportapp.runasp.net/{r.FilePath}" :" ", 
                     Num = r.Number    
                }).FirstOrDefaultAsync();


            if (requestReport == null)
                return RequestResult<GetReportsByidDTO>.Failure(ErrorCode.NoReportFound, "No Report found .");



            return RequestResult<GetReportsByidDTO>.Success(requestReport, "Fetched predefined request categories successfully.");
        }

    }
}
