using CompanyEmloyees.Presentation.Controllers;
using Contracts.Logging;
using Contracts.Managers;
using LoggerService.NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Repositories.Contexts;
using Repositories.Managers;
using Services;
using Services.Contracts;

namespace API.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
        => services.AddCors(opt =>
           {
               opt.AddPolicy("CorsPolicy",
                   builder => builder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .WithExposedHeaders("X-Pagination"));

           });

    public static void ConfigureISSIntegration(this IServiceCollection services)
        => services.Configure<IISOptions>(opt => { });

    public static void ConfigureLoggerService(this IServiceCollection services)
        => services.AddSingleton<ILoggerManager, NLogLoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection servies)
        => servies.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection servies)
      => servies.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqlServer<RepositoryContext>((configuration
        .GetConnectionString("sqlConnection")));

    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

            if (systemTextJsonOutputFormatter !=null)
            {
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.hateoas+json");
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+json");
            }


            var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

            if (xmlOutputFormatter != null)
            {
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.hateoas+json");
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+json");
            }
        });
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions=true;
            opt.AssumeDefaultVersionWhenUnspecified=true;
            opt.DefaultApiVersion=new ApiVersion(1, 0);
            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");

            opt.Conventions.Controller<CompaniesController>()
                .HasApiVersion(new ApiVersion(1, 0));
            opt.Conventions.Controller<EmployeesController>()
                .HasApiVersion(new ApiVersion(2, 0));
        });
    }
}

