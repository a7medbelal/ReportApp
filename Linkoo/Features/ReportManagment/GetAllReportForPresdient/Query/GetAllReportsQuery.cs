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

namespace ReportApp.Features.ReportManagment.GetAllReportForPresdient.Query
{
    public record GetAllReportForPresdientQuery(int pageSize  , int pageIndex, string? KeyWord,ReportStatus? ReportStatus , DateTime? CreatedFrom  ,DateTime? CreatedTo) : IRequest<RequestResult<PagingViewModel<GetAllReportsForPreisdentViewModel>>>;

    public class GetAllReportForPresdientQueryHandler : BaseRequestHandler<GetAllReportForPresdientQuery, RequestResult<PagingViewModel<GetAllReportsForPreisdentViewModel>> , Report>
    {


        public GetAllReportForPresdientQueryHandler(BaseRequestHandlerParamters<Report> paramters) : base(paramters)
        {
        }
        public override async Task<RequestResult<PagingViewModel<GetAllReportsForPreisdentViewModel>>> Handle(GetAllReportForPresdientQuery request, CancellationToken cancellationToken)
        {
            var predicate = Predicate(request);


            var requestList = _repository.Get(predicate).
                 OrderByDescending(r => r.CreatedAt).
                Select(r => new GetAllReportsForPreisdentViewModel
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
                return RequestResult<PagingViewModel<GetAllReportsForPreisdentViewModel>>.Failure(ErrorCode.NoReportFound, "No Report found .");



            return RequestResult<PagingViewModel<GetAllReportsForPreisdentViewModel>>.Success(paginationRequest, "Fetched predefined request categories successfully.");
        }

        private Expression<Func<Report, bool>> Predicate(GetAllReportForPresdientQuery request)
        {
            var user = _userInfo.UserId;
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
