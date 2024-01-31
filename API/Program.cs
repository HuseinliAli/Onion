using API.Extensions;
using API.Utilities;
using AspNetCoreRateLimit;
using CompanyEmloyees.Presentation.ActionFilters;
using Contracts.Hateoas;
using Contracts.Logging;
using Contracts.Shapers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using Repositories.Contexts;
using Services;
using Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentiy();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();
builder.Services.ConfigureCors();
builder.Services.ConfigureISSIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureVersioning();
builder.Services.Configure<ApiBehaviorOptions>(opt =>
    { opt.SuppressModelStateInvalidFilter=true; });
builder.Services.AddCustomMediaTypes();
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader=true;
    config.ReturnHttpNotAcceptable=true;
    config.InputFormatters.Insert(0,GetJsonPatchFormatter());
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration=120 });
}).AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(CompanyEmloyees.Presentation.AssemblyReference).Assembly);
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new()
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");

app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
NewtonsoftJsonPatchInputFormatter GetJsonPatchFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();