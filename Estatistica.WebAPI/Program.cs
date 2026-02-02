using Estatistica.BusinessLogicLayer;
using Estatistica.DataAccessLayer;
using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.WebAPI.Context;
using Estatistica.WebAPI.Middlewares;
using Estatistica.WebAPI.ServiceContracts;
using Estatistica.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<AuditingSaveChangesInterceptorService>();
builder.Services.AddMemoryCache();

//Add DbContext
var connString = string.Empty;

if (builder.Environment.IsDevelopment())
    connString = builder.Configuration.GetConnectionString("test");
else
    connString = builder.Configuration.GetConnectionString("production");


//Add Dal and BLL

builder.Services.AddDataAccessLayer();
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseMySQL(connString!);
    var auditService = serviceProvider.GetRequiredService<AuditingSaveChangesInterceptorService>();
    options.AddInterceptors(auditService);
});


//Add WebApi Services
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme, options =>
    {
        var expirationMinutes = int.Parse(builder.Configuration.GetSection("AppConfigs")["TokenExpireInMinutes"] ?? "120");
        options.BearerTokenExpiration = new TimeSpan(0, expirationMinutes, 0);
    });

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add cors
builder.Services.AddCors();

var app = builder.Build();
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var allowedOrigins = builder.Configuration.GetSection("AppConfigs")["ReactFrontendUrl"]!;
Console.WriteLine($"Running with allowed origins {allowedOrigins}");
app.UseCors(options =>
options.WithOrigins(allowedOrigins)
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials());


app.UseAuthorization();
//Block the access to the register
app.MapIdentityApi<IdentityUser>().AddEndpointFilter(async (efiContext, next) =>
{
    var path = efiContext.HttpContext.Request.Path;
    if (path.Equals("/register", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/refresh", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/confirmEmail", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/resendConfirmationEmail", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/forgotPassword", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/resetPassword", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/manage/2fa", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/manage/info", StringComparison.OrdinalIgnoreCase)
    )
    {
        // Reject access for everyone
        return Results.Forbid();
    }

    return await next(efiContext);
}); ;

app.MapControllers();


app.Run();

