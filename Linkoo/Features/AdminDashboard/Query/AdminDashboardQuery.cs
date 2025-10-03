using MediatR;
using Microsoft.EntityFrameworkCore;
using PredicateExtensions;
using Reportapp.Common.BaseHandler;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Enum;
using ReportApp.Common.Helpers;
using ReportApp.Common.views;
using ReportApp.Common.Views;
using ReportApp.Domain;
using ReportApp.Domain.Repository;
using ReportApp.Features.AdminDashboard;
using ReportApp.Model;
using ReportApp.Model.Enum;
using System.Linq.Expressions;

namespace ReportApp.Features.ReportManagment.GetAllReport.Query
{
    public record AdminDashboardQuery() : IRequest<RequestResult<DashbordViewModel>>;

    public class AdminDashboardQueryHandler : BaseRequestHandler<AdminDashboardQuery, RequestResult<DashbordViewModel> , Report>
    {

        IRepository<AppUser> _userRepository;
        public AdminDashboardQueryHandler(BaseRequestHandlerParamters<Report> paramters, IRepository<AppUser> userRepository) : base(paramters)
        {
            
            _userRepository = userRepository;
        }
        public override async Task<RequestResult<DashbordViewModel>> Handle(AdminDashboardQuery request, CancellationToken cancellationToken)
        {
            var usersGrouped = await _userRepository.GetAll()
            .GroupBy(u => 1)
            .Select(g => new
             {
                UsersCount = g.Count(u => u.role == Role.Agent),
            
            })
            .FirstOrDefaultAsync(cancellationToken);

            var result = new DashbordViewModel
            {
                UsersCount = usersGrouped?.UsersCount ?? 0,
                ApprovedReportsCount = await  _repository.Get(c=>c.Status==ReportStatus.Approved).CountAsync(),
                InProgressReportsCount = await  _repository.Get(c=>c.Status==ReportStatus.Pending).CountAsync(),
                CancelledReportsCount = await  _repository.Get(c=>c.Status==ReportStatus.Rejected).CountAsync(),
                ReportsCount = await  _repository.GetAll().CountAsync()
            };

            return RequestResult<DashbordViewModel>.Success(result, "Dashboard data retrieved successfully.");
        }
    }
}
