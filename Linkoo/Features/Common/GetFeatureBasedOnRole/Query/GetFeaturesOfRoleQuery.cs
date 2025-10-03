using MediatR;
using Microsoft.EntityFrameworkCore;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.views;
using ReportApp.Model;
using ReportApp.Model.Enum;

namespace ReportApp.Features.Common.GetFeatureBasedOnRole.Query;
public record GetFeaturesOfRoleQuery(Role Role, AppFeature Feature) : IRequest<RequestResult<List<AppFeature>>>;

public class HasAccessQueryHandler : BaseRequestHandler<GetFeaturesOfRoleQuery, RequestResult<List<AppFeature>>, RoleFeature>
{
    public HasAccessQueryHandler(BaseRequestHandlerParamters<RoleFeature> parameters) : base(parameters)
    {
    }
    public async override Task<RequestResult<List<AppFeature>>> Handle(GetFeaturesOfRoleQuery request, CancellationToken cancellationToken)
    {
        var features = await _repository
          .Get(c => c.Role == request.Role && !c.IsDeleted)
          .Select(c => c.Feature)
          .ToListAsync();
        if (features == null || !features.Any())
            return RequestResult<List<AppFeature>>.Failure(ErrorCode.RoleNotFound, "Role not found");

        return RequestResult<List<AppFeature>>.Success(features, "Access granted");
    }
}