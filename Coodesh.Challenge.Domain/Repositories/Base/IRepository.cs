using Coodesh.Challenge.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Coodesh.Challenge.Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : Entity
{
    public Task<TEntity> GetByIdAsync(Guid id);

    public Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression);

    public Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression, int take = 100, int startPage = 0);

    public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

    public Task CommitAsync();

    public Task<TEntity> AddAsync(TEntity entity);

    public void Update(TEntity entity);

    public Task RemoveByIdAsync(Guid id);
}