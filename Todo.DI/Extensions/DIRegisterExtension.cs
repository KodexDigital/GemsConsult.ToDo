using Microsoft.Extensions.DependencyInjection;
using Todo.DAL.Interfaces;
using Todo.DAL.Repositories.Declarations;
using Todo.DAL.Repositories.Implementations;
using Todo.Logger.Service.Manager;
using Todo.Logger.Servicer.Manager;
using Todo.Services.Declarations;
using Todo.Services.Implementations;

namespace Todo.DI.Extensions
{
    public static class DIRegisterExtension
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<ITokenizationSetting, TokenizationSetting>();

            services.AddTransient<ILoggerManager, LoggerManager>();

            return services;
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddTransient<ILoggerManager, LoggerManager>();
        }
    }
}
