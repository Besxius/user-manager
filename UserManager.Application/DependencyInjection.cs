using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserManager.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // 1. Đăng ký tự động tất cả các Validator (FluentValidation)
            services.AddValidatorsFromAssembly(assembly);

            // 2. Đăng ký MediatR và Pipeline Behavior
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                // Đăng ký Logging Behavior
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CommandLoggingBehavior<,>));

                // Đăng ký ValidationBehavior
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}
