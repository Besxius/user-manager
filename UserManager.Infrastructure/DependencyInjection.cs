using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Repositories;
using UserManager.Infrastructure.Persistence;
using UserManager.Infrastructure.Repositories;

namespace UserManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(
                configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<MongoDbContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
