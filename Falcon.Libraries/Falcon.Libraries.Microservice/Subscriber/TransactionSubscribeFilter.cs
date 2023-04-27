using DotNetCore.CAP.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Falcon.Libraries.Microservice.Subscriber
{
    public class TransactionSubscribeFilter<TApplicationDbContext> : SubscribeFilter where TApplicationDbContext : DbContext
    {
        private readonly TApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public TransactionSubscribeFilter(TApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnSubscribeExecutingAsync(ExecutingContext context)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();

            await base.OnSubscribeExecutingAsync(context);
        }

        public override async Task OnSubscribeExecutedAsync(ExecutedContext context)
        {
            if (_transaction != null)
            {
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
        }

        public override async Task OnSubscribeExceptionAsync(ExceptionContext context)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}
