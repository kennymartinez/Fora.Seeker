using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fora.Seeker.Web.Infrastructure.Data;

public static class SeedData
{
  public static async Task InitializeAsync(AppDbContext dbContext, ILogger logger)
  {
    // No seed data required for this application
    // Companies will be imported via the API endpoint
    logger.LogInformation("Database initialized. Use POST /Companies/Import to load company data.");
    await Task.CompletedTask;
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext, ILogger logger)
  {
    // No test data to populate
    await Task.CompletedTask;
  }
}
