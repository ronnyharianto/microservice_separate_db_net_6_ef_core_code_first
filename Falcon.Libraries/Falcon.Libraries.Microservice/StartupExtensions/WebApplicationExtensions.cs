using Falcon.Libraries.Common.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Hosting;

namespace Falcon.Libraries.Microservice.Startups
{
	public static class WebApplicationExtensions
	{
		public static WebApplication RunMicroservice<TApplicationDbContext>(this WebApplication app)
			where TApplicationDbContext : DbContext
		{
			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<TApplicationDbContext>();

				if (app.Environment.IsProduction())
				{
					dbContext.Database.Migrate();
				}
				else
				{
					dbContext.Database.EnsureCreated();
				}
			}

			app.UseHttpLogging().UseSerilogRequestLogging();

			app.MapControllers();

			app.Run();

			return app;
		}
	}
}
