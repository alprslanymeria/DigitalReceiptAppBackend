using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence;

public class GenericRepository<TEntity, TId>(AppDbContext context) : IGenericRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    protected readonly AppDbContext Context = context;
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public IQueryable<TEntity> GetAll() => DbSet.AsQueryable().AsNoTracking();

    public async ValueTask<TEntity?> GetByIdAsync(TId id)
    {
        var entity = await DbSet.FindAsync(id);

        if (entity is null) return entity;

        Context.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    public async ValueTask CreateAsync(TEntity entity) => await DbSet.AddAsync(entity);

    public TEntity Update(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public void Delete(TEntity entity) => DbSet.Remove(entity);

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate).AsNoTracking();
}
