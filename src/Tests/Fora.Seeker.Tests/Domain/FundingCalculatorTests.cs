using Fora.Seeker.Web.Domain.CompanyAggregate;

namespace Fora.Seeker.Tests.Domain;

public class FundingCalculatorTests
{
  #region CalculateStandardFundableAmount Tests

  [Fact]
  public void CalculateStandardFundableAmount_WithIncompleteData_ShouldReturnZero()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    // Missing 2020, 2021, 2022

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert
    result.Should().Be(0);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithNegativeIncome2021_ShouldReturnZero()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    company.AddIncome(2020, 2000000m, "10-K", "CY2020");
    company.AddIncome(2021, -500000m, "10-K", "CY2021");
    company.AddIncome(2022, 3000000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert
    result.Should().Be(0);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithZeroIncome2021_ShouldReturnZero()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    company.AddIncome(2020, 2000000m, "10-K", "CY2020");
    company.AddIncome(2021, 0m, "10-K", "CY2021");
    company.AddIncome(2022, 3000000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert
    result.Should().Be(0);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithNegativeIncome2022_ShouldReturnZero()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    company.AddIncome(2020, 2000000m, "10-K", "CY2020");
    company.AddIncome(2021, 2500000m, "10-K", "CY2021");
    company.AddIncome(2022, -1000000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert
    result.Should().Be(0);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithZeroIncome2022_ShouldReturnZero()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    company.AddIncome(2020, 2000000m, "10-K", "CY2020");
    company.AddIncome(2021, 2500000m, "10-K", "CY2021");
    company.AddIncome(2022, 0m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert
    result.Should().Be(0);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithHighIncome_ShouldReturn12Point33Percent()
  {
    // Arrange - Income >= $10B
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 8_000_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 9_000_000_000m, "10-K", "CY2019");
    company.AddIncome(2020, 10_000_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 11_000_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 12_000_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert - 12.33% of highest income (12B)
    var expected = 12_000_000_000m * 0.1233m;
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithExactly10Billion_ShouldReturn12Point33Percent()
  {
    // Arrange - Income exactly $10B
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 8_000_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 9_000_000_000m, "10-K", "CY2019");
    company.AddIncome(2020, 10_000_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 9_500_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 9_800_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert - 12.33% of highest income (10B)
    var expected = 10_000_000_000m * 0.1233m;
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithLowIncome_ShouldReturn21Point51Percent()
  {
    // Arrange - Income < $10B
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert - 21.51% of highest income (3M)
    var expected = 3_000_000m * 0.2151m;
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithJustBelow10Billion_ShouldReturn21Point51Percent()
  {
    // Arrange - Income just below $10B
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 8_000_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 9_000_000_000m, "10-K", "CY2019");
    company.AddIncome(2020, 9_999_999_999m, "10-K", "CY2020");
    company.AddIncome(2021, 9_500_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 9_800_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert - 21.51% of highest income (9,999,999,999)
    var expected = 9_999_999_999m * 0.2151m;
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateStandardFundableAmount_WithHighestIncomeInMiddleYear_ShouldUseHighestIncome()
  {
    // Arrange - Highest income in 2020
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 2_000_000m, "10-K", "CY2019");
    company.AddIncome(2020, 5_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 3_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 4_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateStandardFundableAmount(company);

    // Assert - 21.51% of highest income (5M)
    var expected = 5_000_000m * 0.2151m;
    result.Should().Be(expected);
  }

  #endregion

  #region CalculateSpecialFundableAmount Tests

  [Fact]
  public void CalculateSpecialFundableAmount_WithStandardAmountZero_ShouldReturnZero()
  {
    // Arrange - Missing data will make standard amount 0
    var company = new Company(12345, "Apple Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    result.Should().Be(0);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithVowelStartingName_ShouldAdd15PercentBonus()
  {
    // Arrange - Company name starts with 'A'
    var company = new Company(12345, "Apple Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var expected = standardAmount + (standardAmount * 0.15m);
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithVowelE_ShouldAdd15PercentBonus()
  {
    // Arrange - Company name starts with 'E'
    var company = new Company(12345, "Example Corp");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var expected = standardAmount + (standardAmount * 0.15m);
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithLowercaseVowel_ShouldAdd15PercentBonus()
  {
    // Arrange - Company name starts with lowercase 'a'
    var company = new Company(12345, "apple inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var expected = standardAmount + (standardAmount * 0.15m);
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithNonVowelStartingName_ShouldNotAddBonus()
  {
    // Arrange - Company name starts with 'T'
    var company = new Company(12345, "Tesla Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var expected = 3_000_000m * 0.2151m; // No vowel bonus
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithDecreasingIncome_ShouldApply25PercentPenalty()
  {
    // Arrange - 2022 income < 2021 income
    var company = new Company(12345, "Tesla Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 3_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 2_500_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var expected = standardAmount - (standardAmount * 0.25m);
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithIncreasingIncome_ShouldNotApplyPenalty()
  {
    // Arrange - 2022 income > 2021 income
    var company = new Company(12345, "Tesla Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var expected = 3_000_000m * 0.2151m; // No penalty
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithEqualIncome_ShouldNotApplyPenalty()
  {
    // Arrange - 2022 income == 2021 income
    var company = new Company(12345, "Tesla Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 3_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var expected = 3_000_000m * 0.2151m; // No penalty
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithVowelAndIncreasingIncome_ShouldOnlyAddBonus()
  {
    // Arrange - Vowel + Increasing income
    var company = new Company(12345, "Apple Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 2_500_000m, "10-K", "CY2021");
    company.AddIncome(2022, 3_000_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var expected = standardAmount + (standardAmount * 0.15m); // Only vowel bonus
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithVowelAndDecreasingIncome_ShouldApplyBothBonusAndPenalty()
  {
    // Arrange - Vowel + Decreasing income
    var company = new Company(12345, "Apple Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 3_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 2_500_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var withBonus = standardAmount + (standardAmount * 0.15m);
    var expected = withBonus - (withBonus * 0.25m);
    result.Should().Be(expected);
  }

  [Fact]
  public void CalculateSpecialFundableAmount_WithNonVowelAndDecreasingIncome_ShouldOnlyApplyPenalty()
  {
    // Arrange - Non-vowel + Decreasing income
    var company = new Company(12345, "Tesla Inc.");
    company.AddIncome(2018, 1_000_000m, "10-K", "CY2018");
    company.AddIncome(2019, 1_500_000m, "10-K", "CY2019");
    company.AddIncome(2020, 2_000_000m, "10-K", "CY2020");
    company.AddIncome(2021, 3_000_000m, "10-K", "CY2021");
    company.AddIncome(2022, 2_500_000m, "10-K", "CY2022");

    // Act
    var result = FundingCalculator.CalculateSpecialFundableAmount(company);

    // Assert
    var standardAmount = 3_000_000m * 0.2151m;
    var expected = standardAmount - (standardAmount * 0.25m); // Only penalty
    result.Should().Be(expected);
  }

  #endregion
}
