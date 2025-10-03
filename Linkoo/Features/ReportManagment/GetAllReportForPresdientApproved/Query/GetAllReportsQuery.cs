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

namespace ReportApp.Features.ReportManagment.GetAllReportForPresdientApproved.Query
{
    public record GetAllReportForPresdientApprovedQuery(int pageSize  , int pageIndex, string? KeyWord,ReportStatus? ReportStatus , DateTime? CreatedFrom  ,DateTime? CreatedTo) : IRequest<RequestResult<PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>>>;

    public class GetAllReportForPresdientApprovedQueryHandler : BaseRequestHandler<GetAllReportForPresdientApprovedQuery, RequestResult<PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>> , Report>
    {


        public GetAllReportForPresdientApprovedQueryHandler(BaseRequestHandlerParamters<Report> paramters) : base(paramters)
        {
        }
        public override async Task<RequestResult<PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>>> Handle(GetAllReportForPresdientApprovedQuery request, CancellationToken cancellationToken)
        {
            var predicate = Predicate(request);


            var requestList = _repository.Get(predicate).
                 OrderByDescending(r => r.CreatedAt).
                Select(r => new GetAllReportsForPreisdentApprovedViewModel
                {
                     Id = r.Id,
                     Name =r.CreatedBy.Fname,
                     Title = r.Title,
                     CreatedAt = r.CreatedAt,
                     ApprovedAt = r.ApprovedAt,
                     Num = r.Number, 
                     Status = r.Status.ToString()
                });


            // apply pagination
            var paginationRequest = await PagingHelper.CreateAsync(requestList, request.pageIndex, request.pageSize);


            if (paginationRequest == null)
                return RequestResult<PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>>.Failure(ErrorCode.NoReportFound, "No Report found .");



            return RequestResult<PagingViewModel<GetAllReportsForPreisdentApprovedViewModel>>.Success(paginationRequest, "Fetched predefined request categories successfully.");
        }

        private Expression<Func<Report, bool>> Predicate(GetAllReportForPresdientApprovedQuery request)
        {
            var predicate = PredicateExtensions.PredicateExtensions.Begin<Report>(true);

            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                var term = $"%{request.KeyWord}%";

                predicate = predicate.And(x =>
                EF.Functions.Like(x.Title, term) ||
                EF.Functions.Like(x.Description, term) ||
                EF.Functions.Like(x.Subject, term));


                if (long.TryParse(request.KeyWord , out var number))
                {
                    predicate = predicate.Or(x => x.Number == number);
                }
            }


          
             predicate = predicate.And(x => x.Status != ReportStatus.Pending);

            if (request.CreatedFrom!= null)
                predicate = predicate.And(x => x.CreatedAt >= request.CreatedFrom);

            if (request.CreatedTo.HasValue)
                predicate = predicate.And(x => x.CreatedAt <=  request.CreatedTo);

            return predicate;   
        }

    }
}
