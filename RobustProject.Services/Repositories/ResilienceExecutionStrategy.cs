using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace RobustProject.Services.Repositories;

public class ResilienceExecutionStrategy : ExecutionStrategy
{
    private static readonly int[] DeadlockExceptionErrorCodes = { 1205, 1222 };

    public ResilienceExecutionStrategy(ExecutionStrategyDependencies dependencies, int maxRetryCount, int clientRetryDelayInMilliseconds) : base(dependencies, maxRetryCount, TimeSpan.FromMicroseconds(clientRetryDelayInMilliseconds))
    {
    }

    protected override bool ShouldRetryOn(Exception exception)
    {
        if (exception is DbException dbException)
        {
            return DeadlockExceptionErrorCodes.Contains(dbException.ErrorCode) || dbException.ErrorCode < 0;
        }

        return false;
    }
}