using App.Application.Contracts.Persistence;

namespace App.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public Task<int> CommitAsync() => _context.SaveChangesAsync();
}