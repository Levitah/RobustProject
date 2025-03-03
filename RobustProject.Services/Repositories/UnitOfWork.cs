using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace RobustProject.Services.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private DbContext _dbContext;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _dbContext = null!;
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await transaction.RollbackAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellarionToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellarionToken);
    }
}