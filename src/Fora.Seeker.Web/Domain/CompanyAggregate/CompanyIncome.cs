using Ardalis.GuardClauses;

namespace Fora.Seeker.Web.Domain.CompanyAggregate;

public class CompanyIncome
{
  public int Id { get; private set; }
  public int CompanyId { get; private set; }
  public int Year { get; private set; }
  public decimal IncomeAmount { get; private set; }
  public string Form { get; private set; } = string.Empty;
  public string Frame { get; private set; } = string.Empty;

  // Navigation property
  public Company Company { get; private set; } = null!;

  // EF Core constructor
  private CompanyIncome() { }

  public CompanyIncome(int year, decimal incomeAmount, string form, string frame)
  {
    Year = Guard.Against.NegativeOrZero(year);
    IncomeAmount = incomeAmount; // Can be negative
    Form = Guard.Against.NullOrWhiteSpace(form);
    Frame = Guard.Against.NullOrWhiteSpace(frame);
  }

  internal void UpdateIncome(decimal incomeAmount, string form, string frame)
  {
    IncomeAmount = incomeAmount;
    Form = Guard.Against.NullOrWhiteSpace(form);
    Frame = Guard.Against.NullOrWhiteSpace(frame);
  }
}
