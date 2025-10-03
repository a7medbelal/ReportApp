
using Microsoft.AspNetCore.Mvc.Filters;
using ReportApp.Common.views;
using ReportApp.Model.Enum;
using System.Security.Claims;

namespace ReportApp.Filters
{

    public class UserInfoFilter : IActionFilter
    {
        private readonly UserInfoProvider _userInfoProvider; 
        public UserInfoFilter(UserInfoProvider userInfoProvider)
        {
            _userInfoProvider = userInfoProvider;
        }


        public void OnActionExecuted(ActionExecutedContext context)
            {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
                var userInfo = context.HttpContext.User;
            if (userInfo != null)
            {
                var userId = userInfo.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = userInfo.FindFirst("roleType")?.Value;
                var name  = userInfo.FindFirst(ClaimTypes.Name)?.Value; 

                if (userId == null) userId = "-1";
                if (userRole == null) userRole = "-1";

                _userInfoProvider.UserInfo = new UserInfo { UserId = userId, Role = userRole , UserName = name };
            }
        }
    }
}
