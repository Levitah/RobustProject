using Microsoft.EntityFrameworkCore.Storage;

namespace RobustProject.Services.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangeAsync(CancellationToken cancellarionToken = default);

    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(IDbContextTransaction tramsaction, CancellationToken cancellationToken = default);
}