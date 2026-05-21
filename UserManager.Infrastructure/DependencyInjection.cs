using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Authentication;
using UserManager.Application.Abstractions.Cryptography;
using UserManager.Domain.Repositories;
using UserManager.Infrastructure.Authentication;
using UserManager.Infrastructure.Cryptography;
using UserManager.Infrastructure.Persistence;
using UserManager.Infrastructure.Repositories;

namespace UserManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", pack, t => true);

            services.Configure<MongoDbSettings>(
                configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<MongoDbContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings?.Issuer,
                        ValidAudience = jwtSettings?.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings?.Secret ?? string.Empty))
                    };
                });

            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddTransient<MongoDataSeeder>();

            return services;
        }
    }
}
