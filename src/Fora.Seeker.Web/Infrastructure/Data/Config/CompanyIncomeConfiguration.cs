using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fora.Seeker.Web.Domain.CompanyAggregate;

namespace Fora.Seeker.Web.Infrastructure.Data.Config;

public class CompanyIncomeConfiguration : IEntityTypeConfiguration<CompanyIncome>
{
  public void Configure(EntityTypeBuilder<CompanyIncome> builder)
  {
    builder.ToTable("CompanyIncomes");

    builder.HasKey(i => i.Id);

    builder.Property(i => i.CompanyId)
      .IsRequired();

    builder.Property(i => i.Year)
      .IsRequired();

    builder.Property(i => i.IncomeAmount)
      .IsRequired()
      .HasColumnType("decimal(18,2)");

    builder.Property(i => i.Form)
      .IsRequired()
      .HasMaxLength(50);

    builder.Property(i => i.Frame)
      .IsRequired()
      .HasMaxLength(50);

    builder.HasIndex(i => new { i.CompanyId, i.Year })
      .IsUnique();

    builder.HasIndex(i => i.Year);
  }
}
