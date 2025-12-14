using Ardalis.GuardClauses;
using Fora.Seeker.Web.Infrastructure.Data;
using Fora.Seeker.Web.Infrastructure.Edgar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fora.Seeker.Web.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    // Always use SQL Server from Aspire
    string? connectionString = config.GetConnectionString("AppDb");
    Guard.Against.Null(connectionString, "AppDb connection string is required. Make sure the application is running with Aspire.");

    services.AddScoped<EventDispatchInterceptor>();
    services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

    services.AddDbContext<AppDbContext>((provider, options) =>
    {
      var eventDispatchInterceptor = provider.GetRequiredService<EventDispatchInterceptor>();
      
      options.UseSqlServer(connectionString);
      options.AddInterceptors(eventDispatchInterceptor);
    });

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

    // Register EDGAR API Service with HttpClient
    services.AddHttpClient<IEdgarApiService, EdgarApiService>(client =>
    {
      client.BaseAddress = new Uri("https://data.sec.gov/");
      client.Timeout = TimeSpan.FromSeconds(30);
    });

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
