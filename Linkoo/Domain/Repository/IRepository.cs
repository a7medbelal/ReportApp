using ReportApp.Model;
using System.Linq.Expressions;

namespace ReportApp.Domain.Repository
{
    public interface IRepository<Entity> where Entity : BaseModel
    {
       Task<Entity> GetByIdAsync(Guid id);
       Task  AddAsync(Entity entity);
        Task<bool> UpdateAsync(Entity entity, params string[] proprties);
       Task DeleteAsync(Entity entity);
       IQueryable<Entity> GetAll();
       IQueryable<Entity> Get(Expression<Func<Entity, bool >> Perdiacte);
       Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
       Task<int> CountAsync(Expression<Func<Entity, bool>> predicate);
       Task SaveChangesAsync();
       Task AddRangeAsync(IEnumerable<Entity> entities);
       Task UpdateRangeAsync(IEnumerable<Entity> entities);
        Task OldUpdateAsync(Entity entity);
    }
}
