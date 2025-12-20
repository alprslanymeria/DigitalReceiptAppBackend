using App.Domain.Entities;

namespace App.Domain.Events;

public class EntityDeletedEvent<T, TId>(T entity) where T : BaseEntity<TId>
{
    public T Entity { get; } = entity;
}
