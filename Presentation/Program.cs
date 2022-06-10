using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities;
using Todo.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Todo.Core.Settings;
using Todo.DI.Extensions;
using Presentation.Middleware;
using NLog;
using Todo.Logger.Servicer.Manager;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Dbcon");
builder.Services.AddDbContextPool<TodoDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Stores.ProtectPersonalData = false;
    opt.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<TodoDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(5);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});


//Add Dependency for logger service
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

//Add Dependency
builder.Services.InjectDependencies();

// configure strongly typed settings object
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


builder.Services.AddApiVersioning(v =>
{
    v.AssumeDefaultVersionWhenUnspecified = true;
    v.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    v.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerManager>());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
