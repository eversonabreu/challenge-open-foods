using Coodesh.Challenge.Domain.Entities.Base;
using Coodesh.Challenge.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace Coodesh.Challenge.Infrastructure.Database.Repositories.Base;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly DatabaseContext databaseContext;
    private readonly DbSet<TEntity> dbSet;

    public Repository(IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetSection("DbConnectionString").Value;
        databaseContext = DatabaseContext.Create(dbConnectionString);
        dbSet = databaseContext.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        var entity = await dbSet.SingleOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new Exception($"'{id}' not found.");
        }

        return entity;
    }

    public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await dbSet.SingleOrDefaultAsync(expression);
        return entity;
    }

    public async Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entities = await dbSet.Where(expression)?.ToListAsync();

        if (entities == null)
        {
            return new List<TEntity>();
        }

        return entities;
    }

    public async Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression, int take = 100, int startPage = 0)
    {
        var entities = dbSet.Where(expression);

        if (entities == null)
        {
            return new List<TEntity>();
        }

        var skip = startPage * take;
        var result = await entities.Skip(skip).Take(take).ToListAsync();

        return result;
    }

    public async Task CommitAsync() => await databaseContext.SaveChangesAsync();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.Id = Guid.NewGuid();
        await dbSet.AddAsync(entity);
        return entity;
    }

    public void Update(TEntity entity) => dbSet.Update(entity);

    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        dbSet.Remove(entity);
    }
}