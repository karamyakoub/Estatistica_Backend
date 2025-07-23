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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<AuditingSaveChangesInterceptorService>();

//Add DbContext
var connString = string.Empty;

if (builder.Environment.IsDevelopment())
    connString = builder.Configuration.GetConnectionString("test");
else
    connString = builder.Configuration.GetConnectionString("production");


//Add Dal and BLL

builder.Services.AddDataAccessLayer(connString!);
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
        options.BearerTokenExpiration = new TimeSpan(0, 15, 0);
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add cors
builder.Services.AddCors();

//Hosted services
//builder.Services.AddHostedService<CarregaProdutosHostedService>();
builder.Services.AddHostedService<PlanilhaProcessingService>();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
//Block the access to the register
app.MapIdentityApi<IdentityUser>().AddEndpointFilter(async (efiContext, next) =>
{
    var path = efiContext.HttpContext.Request.Path;
    if (path.Equals("/register", StringComparison.OrdinalIgnoreCase))
    {
        // Reject access for everyone
        return Results.Forbid();
    }

    return await next(efiContext);
}); ;

app.MapControllers();
app.UseCors(options => 
options.WithOrigins(
    builder.Configuration.GetSection("AppConfigs")["ReactFrontendUrl"]!)
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials());

app.Run();

