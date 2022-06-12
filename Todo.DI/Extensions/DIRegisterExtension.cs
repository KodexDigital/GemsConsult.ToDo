using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
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

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Gems-Consult Todo Service",
                    Version = "1.0",
                    Description = "This is a standardized service for organizing what to do.",
                    Contact = new OpenApiContact
                    {
                        Name = "Kenneth Otoro",
                        Email = "kodexkenth@gmail.com",
                        Url = new Uri("https://github.com/KodexDigital/GemsConsult.ToDo")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License:MIT",
                    }
                });
                c.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Authorization format : Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.SaveToken = true;
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:authKey"))),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("JwtSettings:authIssuer"),
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<string>("JwtSettings:authAudience"),
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }
    }
}
