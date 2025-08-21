using ApiGateway;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            // .WithOrigins("http://localhost:3000")
            //  .AllowAnyOrigin()
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("x-pagination"));
});

builder.Services.AddSignalR();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var configuration = context.GetRequiredService<IConfiguration>();
        var rabbitMqSection = configuration.GetSection("RabbitMq");
        var host = rabbitMqSection["Host"] ?? "localhost";
        var username = rabbitMqSection["Username"] ?? "guest";
        var password = rabbitMqSection["Password"] ?? "guest";

        cfg.Host(host, 5672, rabbitMqSection["VirtualHost"] ?? "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });

        cfg.ReceiveEndpoint("order-created-gateway-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapReverseProxy();
app.MapHub<NotificationHub>("/hub/notifications");
app.Run();