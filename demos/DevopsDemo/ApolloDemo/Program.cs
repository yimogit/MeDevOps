var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddApollo(builder.Configuration.GetSection("apollo"));
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/config", context =>
{
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    //配置服务
    var configService = context.RequestServices.GetRequiredService<IConfiguration>();
    string? key = context.Request.Query["key"];
    if (string.IsNullOrWhiteSpace(key))
    {
        return context.Response.WriteAsync("获取配置：/config?key=test");
    }
    var value = configService[key];
    return context.Response.WriteAsync(value ?? "undefined");
});

app.Run();