namespace Fora.Seeker.Web.Domain.CompanyAggregate;

public static class FundingCalculator
{
  private const int RequiredStartYear = 2018;
  private const int RequiredEndYear = 2022;
  private const decimal TenBillionThreshold = 10_000_000_000m;
  private const decimal HighIncomePercentage = 0.1233m; // 12.33%
  private const decimal LowIncomePercentage = 0.2151m; // 21.51%
  private const decimal VowelBonus = 0.15m; // 15%
  private const decimal DecreasingIncomePenalty = 0.25m; // 25%

  public static decimal CalculateStandardFundableAmount(Company company)
  {
    // Must have income data for all years 2018-2022
    if (!company.HasCompleteDataForYears(RequiredStartYear, RequiredEndYear))
    {
      return 0;
    }

    // Must have positive income in both 2021 and 2022
    var income2021 = company.GetIncomeForYear(2021);
    var income2022 = company.GetIncomeForYear(2022);

    if (income2021 <= 0 || income2022 <= 0)
    {
      return 0;
    }

    // Find highest income between 2018 and 2022
    decimal highestIncome = 0;
    for (int year = RequiredStartYear; year <= RequiredEndYear; year++)
    {
      var yearIncome = company.GetIncomeForYear(year);
      if (yearIncome > highestIncome)
      {
        highestIncome = yearIncome;
      }
    }

    // Calculate based on threshold
    if (highestIncome >= TenBillionThreshold)
    {
      return highestIncome * HighIncomePercentage;
    }
    else
    {
      return highestIncome * LowIncomePercentage;
    }
  }

  public static decimal CalculateSpecialFundableAmount(Company company)
  {
    // Start with standard amount
    decimal specialAmount = CalculateStandardFundableAmount(company);

    // If standard is 0, special is also 0
    if (specialAmount == 0)
    {
      return 0;
    }

    // Check if company name starts with a vowel
    if (StartsWithVowel(company.Name))
    {
      specialAmount += specialAmount * VowelBonus;
    }

    // Check if 2022 income was less than 2021 income
    var income2021 = company.GetIncomeForYear(2021);
    var income2022 = company.GetIncomeForYear(2022);

    if (income2022 < income2021)
    {
      specialAmount -= specialAmount * DecreasingIncomePenalty;
    }

    return specialAmount;
  }

  private static bool StartsWithVowel(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return false;
    }

    char firstChar = char.ToUpperInvariant(name[0]);
    return firstChar == 'A' || firstChar == 'E' || firstChar == 'I' || 
           firstChar == 'O' || firstChar == 'U';
  }
}
