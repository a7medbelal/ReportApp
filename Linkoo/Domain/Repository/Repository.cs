using Linkoo.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Linkoo.Domain.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : Model.BaseModel
    {
            protected Context _context;
            private readonly DbSet<Entity> _dbSet;
            private readonly string[] immutableProps = { nameof(BaseModel.Id), nameof(BaseModel.CreatedBy), nameof(BaseModel.CreatedAt) };
            public Repository(Context context )
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _dbSet = _context.Set<Entity>();
            }

            public async Task AddAsync(Entity entity)
            {
              entity.CreatedAt = DateTime.UtcNow;
              await _dbSet.AddAsync(entity);    
            }

            public Task AddRangeAsync(IEnumerable<Entity> entities)
            {
                foreach (var entity in entities)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                return _dbSet.AddRangeAsync(entities);

            }

            public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
            {
               return await Get(predicate).AnyAsync();
            }

            public Task<int> CountAsync(Expression<Func<Entity, bool>> predicate)
            {
                throw new NotImplementedException();
            }

            public async Task DeleteAsync(Entity entity)
            { 
               await _dbSet.Where(e => e.Id == entity.Id)
                    .ExecuteUpdateAsync(x=> x.SetProperty(xx => xx.IsDeleted , true)
                    .SetProperty(z=> z.UpdatedAt , DateTime.Now));
            }

            public IQueryable<Entity> Get(Expression<Func<Entity, bool>> Perdiacte)
            {
                return GetAll().Where(Perdiacte);
            }

            public IQueryable<Entity> GetAll()
            {
               return _dbSet.Where(x => !x.IsDeleted);
            }

            public async Task<Entity> GetByIdAsync(int id)
            {
                return await Get(x => x.Id == id).FirstOrDefaultAsync(); 
            }

            public async Task SaveChangesAsync()
            {
               await _context.SaveChangesAsync();
            }

            public async Task<bool> UpdateAsync(Entity entity ,params string[] proprties )
            {
                try
                {
                    var localEntity = _dbSet.Local.FirstOrDefault(x => x.Id == entity.Id);
                    EntityEntry entry;
                    if (localEntity != null)
                    {
                        entry = _context.Entry(localEntity);
                    }
                    else
                    {
                        _dbSet.Attach(entity);
                        entry = _context.Entry(entity);
                    }
                    if (entry == null)
                    {
                        return false;
                    }
                    foreach (var prop in entry.Properties)
                    {
                        if (proprties.Contains(prop.Metadata.Name) && !immutableProps.Contains(prop.Metadata.Name))
                        {
                            prop.CurrentValue = entity.GetType().GetProperty(prop.Metadata.Name)?.GetValue(entity);
                            prop.IsModified = true;

                        }
                    }
                    entity.UpdatedAt = DateTime.UtcNow;
                    entry.Property(nameof(entity.UpdatedBy)).IsModified = true;
                    return true;


                }

                catch (Exception ex) {
                    throw new Exception($"Error updating entity: {ex.Message}", ex);
                }
            }

            public Task UpdateRangeAsync(IEnumerable<Entity> entities)
            {
                throw new NotImplementedException();
            }
    }
}
