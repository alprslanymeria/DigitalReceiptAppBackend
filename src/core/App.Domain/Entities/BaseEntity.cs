namespace App.Domain.Entities;

public class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
}
