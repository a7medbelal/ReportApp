
using Linkoo.Common.Helper;
using Linkoo.Common.views;
using Linkoo.Domain.Repository;
using Linkoo.Model;
using MediatR;

namespace Linkoo.Common.BaseHandler
{
    public class BaseRequestHandlerParamters<TEntity>
    where TEntity : BaseModel
    {
        private readonly IMediator _mediator;
        private readonly IRepository<TEntity> _repository;
        private readonly TokenHelper _tokenHelper;
        private readonly UserInfo _userInfo;

        public IMediator Mediator => _mediator;
        public IRepository<TEntity> Repository => _repository;
        public TokenHelper TokenHelper => _tokenHelper;
        public UserInfo UserInfo => _userInfo;
        public BaseRequestHandlerParamters(IMediator mediator, IRepository<TEntity> repository, TokenHelper tokenHelper, UserInfoProvider userInfoProvider)
        {
            _mediator = mediator;
            _repository = repository;
            _tokenHelper = tokenHelper;
            _userInfo = userInfoProvider.UserInfo;
        }
    }
}
