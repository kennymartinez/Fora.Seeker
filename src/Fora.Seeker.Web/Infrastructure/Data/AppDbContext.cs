using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Fora.Seeker.Web.Domain.CompanyAggregate;

namespace Fora.Seeker.Web.Infrastructure.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : 
  DbContext(options)
{
  public DbSet<Company> Companies => Set<Company>();
  public DbSet<CompanyIncome> CompanyIncomes => Set<CompanyIncome>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
