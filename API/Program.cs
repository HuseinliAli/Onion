using API.Extensions;
using Contracts.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureISSIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<ApiBehaviorOptions>(opt =>
    { opt.SuppressModelStateInvalidFilter=true; });
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader=true;
    config.ReturnHttpNotAcceptable=true;
    config.InputFormatters.Insert(0,GetJsonPatchFormatter());   
}).AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(CompanyEmloyees.Presentation.AssemblyReference).Assembly);

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

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
NewtonsoftJsonPatchInputFormatter GetJsonPatchFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();