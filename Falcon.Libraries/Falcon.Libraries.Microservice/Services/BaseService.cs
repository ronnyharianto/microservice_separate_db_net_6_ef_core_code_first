using Microsoft.EntityFrameworkCore;

namespace Falcon.Libraries.Microservice.Services
{
    public class BaseService<TApplicationDbContext> where TApplicationDbContext : DbContext
    {
        protected readonly TApplicationDbContext _dbContext;

        public BaseService(TApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
