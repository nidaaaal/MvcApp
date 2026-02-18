using MvcApp;
using MvcApp.Helper;
using MvcApp.Middleware;
using MvcApp.Repository;
using MvcApp.Repository.Interfaces;
using MvcApp.Services;
using MvcApp.Services.Interfaces;
using MvcApp.Extensions;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddSession( option=>{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;

});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserFinder, UserFinder>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IAdminServices, AdminServices>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(x => x.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 31,
        shared: true,
        outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss} | {Level:u3} | {Message:1j}{NewLine}{Exception}"
        )).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();
app.UseUserIdMiddleware();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
