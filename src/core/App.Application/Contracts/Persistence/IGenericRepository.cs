using App.Domain.Entities;
using System.Linq.Expressions;

namespace App.Application.Contracts.Persistence;

public interface IGenericRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    IQueryable<TEntity> GetAll();
    ValueTask<TEntity?> GetByIdAsync(TId id);
    ValueTask CreateAsync(TEntity entity);
    TEntity Update(TEntity entity);
    void Delete(TEntity entity);
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
}
