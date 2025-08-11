
using Linkoo.Common.BaseHandler;
using Linkoo.Common.Helper;
using Linkoo.Common.views;
using Linkoo.Domain.Repository;
using Linkoo.Model;
using MediatR;

namespace Linko.Common.BaseHandler
{
    public abstract class BaseRequestHandler<TRequest, TResponse, TEntity> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
        where TEntity : BaseModel
    {
        protected readonly IMediator _mediator;
        protected readonly IRepository<TEntity> _repository;
        protected readonly TokenHelper _tokenHelper;
        protected readonly UserInfo _userInfo;

        public BaseRequestHandler(BaseRequestHandlerParamters<TEntity> paramters) 
        {
            _mediator = paramters.Mediator;
            _repository = paramters.Repository;
            _tokenHelper = paramters.TokenHelper;
            _userInfo = paramters.UserInfo;
        }
        public abstract  Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
