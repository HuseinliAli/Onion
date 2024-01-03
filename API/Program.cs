using API.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureISSIntegration();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new()
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.Use(async (context, next) =>
{
    Console.WriteLine("Login before executing the next delegate in the Use method");
    await next.Invoke();
    Console.WriteLine("Login after executing the next delegate in the Use method");

});

app.Map("/usingmapbranch", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Map branch logic in the Use method before the next delegate");
        await next.Invoke();
        Console.WriteLine("Map branch logic in the Use method after the next delegate");
    });

    builder.Run(async context =>
    {
        Console.WriteLine("Map branch response to the client in the Run method");
        await context.Response.WriteAsync("Hello from the map branch");

    });
});

app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder
    =>
{
    builder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from MapWhen branch");
    });
});

app.Run(async context =>
{
    Console.WriteLine("Writing the response to the client in the Run method");
    await context.Response.WriteAsync("Hello from middleware component");
});

app.MapControllers();

app.Run();
