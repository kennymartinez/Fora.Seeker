using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fora.Seeker.Web.Domain.CompanyAggregate;

namespace Fora.Seeker.Web.Infrastructure.Data.Config;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
  public void Configure(EntityTypeBuilder<Company> builder)
  {
    builder.ToTable("Companies");

    builder.HasKey(c => c.Id);

    builder.Property(c => c.Cik)
      .IsRequired();

    builder.HasIndex(c => c.Cik)
      .IsUnique();

    builder.Property(c => c.Name)
      .IsRequired()
      .HasMaxLength(500);

    builder.HasMany(c => c.Incomes)
      .WithOne(i => i.Company)
      .HasForeignKey(i => i.CompanyId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
