using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Falcon.Libraries.Microservice.Services
{
    public class BaseService<TApplicationDbContext> where TApplicationDbContext : DbContext
    {
        protected readonly TApplicationDbContext _dbContext;
        protected readonly IMapper _mapper;

        public BaseService(TApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
