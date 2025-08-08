var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            // .WithOrigins("http://localhost:3000")
            .AllowAnyOrigin()
            .AllowAnyHeader()
			.WithExposedHeaders("x-pagination")
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapReverseProxy();

app.Run();