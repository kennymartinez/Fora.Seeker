using Ardalis.GuardClauses;

namespace Fora.Seeker.Web.Domain.CompanyAggregate;

public class Company
{
  public int Id { get; private set; }
  public int Cik { get; private set; }
  public string Name { get; private set; } = string.Empty;
  
  private readonly List<CompanyIncome> _incomes = new();
  public IReadOnlyCollection<CompanyIncome> Incomes => _incomes.AsReadOnly();

  // EF Core constructor
  private Company() { }

  public Company(int cik, string name)
  {
    Cik = Guard.Against.NegativeOrZero(cik);
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  public void AddIncome(int year, decimal incomeAmount, string form, string frame)
  {
    var existingIncome = _incomes.FirstOrDefault(i => i.Year == year);
    if (existingIncome != null)
    {
      // Update existing income if found
      existingIncome.UpdateIncome(incomeAmount, form, frame);
    }
    else
    {
      _incomes.Add(new CompanyIncome(year, incomeAmount, form, frame));
    }
  }

  public decimal GetIncomeForYear(int year)
  {
    return _incomes.FirstOrDefault(i => i.Year == year)?.IncomeAmount ?? 0;
  }

  public bool HasCompleteDataForYears(int startYear, int endYear)
  {
    for (int year = startYear; year <= endYear; year++)
    {
      if (!_incomes.Any(i => i.Year == year))
      {
        return false;
      }
    }
    return true;
  }
}
