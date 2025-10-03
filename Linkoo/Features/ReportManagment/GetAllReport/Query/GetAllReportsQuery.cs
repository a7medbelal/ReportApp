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

namespace ReportApp.Features.ReportManagment.GetAllReport.Query
{
    public record GetAllReportsQuery(int pageSize  , int pageIndex, string? KeyWord,ReportStatus? ReportStatus , DateTime? CreatedFrom  ,DateTime? CreatedTo) : IRequest<RequestResult<PagingViewModel<GetAllReportsDTO>>>;

    public class GetAllRequestTypesQureyHandler : BaseRequestHandler<GetAllReportsQuery, RequestResult<PagingViewModel<GetAllReportsDTO>> , Report>
    {


        public GetAllRequestTypesQureyHandler(BaseRequestHandlerParamters<Report> paramters) : base(paramters)
        {
        }
        public override async Task<RequestResult<PagingViewModel<GetAllReportsDTO>>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {
            var predicate = Predicate(request);


            var requestList = _repository.Get(predicate).Where(c =>  c.CreatedById == Guid.Parse(_userInfo.UserId)).
                OrderByDescending(r => r.CreatedAt).
                Select(r => new GetAllReportsDTO
                {
                     Id = r.Id,
                     Title = r.Title,
                     CreatedAt = r.CreatedAt,
                     ApprovedAt = r.ApprovedAt,
                     Status = r.Status.ToString()
                });


            // apply pagination
            var paginationRequest = await PagingHelper.CreateAsync(requestList, request.pageIndex, request.pageSize);


            if (paginationRequest == null)
                return RequestResult<PagingViewModel<GetAllReportsDTO>>.Failure(ErrorCode.NoReportFound, "No Report found .");



            return RequestResult<PagingViewModel<GetAllReportsDTO>>.Success(paginationRequest, "Fetched predefined request categories successfully.");
        }

        private Expression<Func<Report, bool>> Predicate(GetAllReportsQuery request)
        {
            var predicate = PredicateExtensions.PredicateExtensions.Begin<Report>(true);

            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                var term = $"%{request.KeyWord}%";

                predicate = predicate.And(x =>
                EF.Functions.Like(x.Title, term) ||
                EF.Functions.Like(x.Description, term) ||
                EF.Functions.Like(x.Subject, term));
            }


            if (request.ReportStatus.HasValue)
            {
                predicate = predicate.And(x => x.Status == request.ReportStatus);
            }

            if (request.CreatedFrom!= null)
                predicate = predicate.And(x => x.CreatedAt >= request.CreatedFrom);

            if (request.CreatedTo.HasValue)
                predicate = predicate.And(x => x.CreatedAt <=  request.CreatedTo);

            return predicate;   
        }

    }
}
