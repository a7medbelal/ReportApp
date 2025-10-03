using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using ReportApp.Features.Common.GetFeatureBasedOnRole.Query;
using ReportApp.Model.Enum;
using System.Threading.Tasks;

namespace ReportApp.Filters
{
    public class CustomizeAuthorizeAttribute : ActionFilterAttribute
    {
        AppFeature _feature;
        IMediator _mediator;
        IMemoryCache _memoryCache;

        public CustomizeAuthorizeAttribute(AppFeature feature, IMediator mediator , IMemoryCache memoryCache )
        {
            _feature = feature;
            _mediator = mediator;
            _memoryCache = memoryCache;
        }


        public override  void OnActionExecuting(ActionExecutingContext context)
        {
            var claims = context.HttpContext.User;

            var RoleID = claims.FindFirst("roleType");

            if (RoleID == null || string.IsNullOrEmpty(RoleID.Value))
            {

                throw new UnauthorizedAccessException();
            }


            var role = (Role)int.Parse(RoleID.Value);

            //check if the feature per role not the request cach 
            if (!context.HttpContext.Items.TryGetValue("UserFeature", out var FeaturesWithRequest))
            {
                //check if the feature not been in the Imemorycach 
                if (!_memoryCache.TryGetValue(role, out List<AppFeature> FeatureCaches))
                {
                    var hasAccess =  _mediator.Send(new GetFeaturesOfRoleQuery(role, _feature)).Result;
                  
                    if (!hasAccess.isSuccess)
                        throw new UnauthorizedAccessException();

                    FeatureCaches = hasAccess.data; 

                    _memoryCache.Set(role, FeatureCaches , TimeSpan.FromMinutes(30));
                        
                }
                //set the feature in the request
                FeaturesWithRequest = FeatureCaches;
                context.HttpContext.Items["UserFeature"] = FeaturesWithRequest;
            }


            var UserFeature = FeaturesWithRequest as List<AppFeature>;
            if (!UserFeature.Contains(_feature))
            {
                throw new UnauthorizedAccessException();
            }

            base.OnActionExecuting(context);
        }
    }
}
