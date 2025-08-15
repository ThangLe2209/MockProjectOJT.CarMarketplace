using CarMarketplace.Contracts.Logging;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.CQRS.Car.Handler;
using OrderApi.Application.CQRS.Car.Validation;
using Polly;
using Polly.Retry;
using Refit;

namespace OrderApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly));
            services.AddValidatorsFromAssemblyContaining<OrderInputValidator>();

            //// Register HttpClient service
            //// Create Dependency Injection
            //services.AddHttpClient<IUserService, UserService>(options =>
            //{
            //    options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
            //    options.Timeout = TimeSpan.FromSeconds(3); // after 3 second request handle not success will throw TaskCanceledException
            //});

            // Register Refit client
            //services.AddRefitClient<IAuthenticationApi>()
            //    .ConfigureHttpClient(c =>
            //    {
            //        c.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
            //        c.Timeout = TimeSpan.FromSeconds(3);
            //    });

            // Register your UserService (injects IAuthenticationApi)
            //services.AddScoped<IUserService, UserService>();


            // Create Retry Strategy
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(), // TaskCanceledException here
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    LogException.LogToFile(message);
                    return ValueTask.CompletedTask;
                }
            };

            // Use Retry strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}
