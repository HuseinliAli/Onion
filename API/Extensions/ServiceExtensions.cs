using Microsoft.AspNetCore.Builder;

namespace API.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
        => services.AddCors(opt =>
           {
            opt.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
           });

    public static void ConfigureISSIntegration(this IServiceCollection services)
        => services.Configure<IISOptions>(opt => { });
}

