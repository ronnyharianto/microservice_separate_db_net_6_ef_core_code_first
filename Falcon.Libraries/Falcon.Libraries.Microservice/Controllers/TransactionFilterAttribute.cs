using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Falcon.Libraries.Microservice.Controllers
{
    public class TransactionFilterAttribute<TApplicationDbContext> : IAsyncActionFilter where TApplicationDbContext : DbContext
    {
        private readonly TApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly JsonHelper _jsonHelper;

        public TransactionFilterAttribute(TApplicationDbContext dbContext, ILogger<TransactionFilterAttribute<TApplicationDbContext>> logger, JsonHelper jsonHelper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _jsonHelper = jsonHelper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var resultContext = await next();

                if (resultContext.Result != null && resultContext.Exception == null)
                {
                    if (((ObjectResult)resultContext.Result).Value is not ServiceResult)
                    {
                        resultContext.Result = new JsonResult(new ServiceResult(ServiceResultCode.Error, "Use ServiceResult or it's inheritance"));
                        throw new Exception();
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
