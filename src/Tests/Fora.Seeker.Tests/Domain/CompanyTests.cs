using Fora.Seeker.Web.Domain.CompanyAggregate;

namespace Fora.Seeker.Tests.Domain;

public class CompanyTests
{
  [Fact]
  public void Constructor_WithValidParameters_ShouldCreateCompany()
  {
    // Arrange
    var cik = 12345;
    var name = "Test Company";

    // Act
    var company = new Company(cik, name);

    // Assert
    company.Cik.Should().Be(cik);
    company.Name.Should().Be(name);
    company.Incomes.Should().BeEmpty();
  }

  [Fact]
  public void Constructor_WithZeroCik_ShouldThrowException()
  {
    // Arrange
    var cik = 0;
    var name = "Test Company";

    // Act
    var act = () => new Company(cik, name);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithNegativeCik_ShouldThrowException()
  {
    // Arrange
    var cik = -100;
    var name = "Test Company";

    // Act
    var act = () => new Company(cik, name);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithNullName_ShouldThrowException()
  {
    // Arrange
    var cik = 12345;
    string name = null!;

    // Act
    var act = () => new Company(cik, name);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithEmptyName_ShouldThrowException()
  {
    // Arrange
    var cik = 12345;
    var name = "";

    // Act
    var act = () => new Company(cik, name);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithWhitespaceName_ShouldThrowException()
  {
    // Arrange
    var cik = 12345;
    var name = "   ";

    // Act
    var act = () => new Company(cik, name);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void AddIncome_WithNewYear_ShouldAddIncomeToCollection()
  {
    // Arrange
    var company = new Company(12345, "Test Company");

    // Act
    company.AddIncome(2021, 1000000m, "10-K", "CY2021");

    // Assert
    company.Incomes.Should().HaveCount(1);
    company.Incomes.First().Year.Should().Be(2021);
    company.Incomes.First().IncomeAmount.Should().Be(1000000m);
    company.Incomes.First().Form.Should().Be("10-K");
    company.Incomes.First().Frame.Should().Be("CY2021");
  }

  [Fact]
  public void AddIncome_WithExistingYear_ShouldUpdateIncome()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2021, 1000000m, "10-K", "CY2021");

    // Act
    company.AddIncome(2021, 2000000m, "10-K", "CY2021");

    // Assert
    company.Incomes.Should().HaveCount(1);
    company.Incomes.First().Year.Should().Be(2021);
    company.Incomes.First().IncomeAmount.Should().Be(2000000m);
  }

  [Fact]
  public void AddIncome_WithMultipleYears_ShouldAddAllIncomes()
  {
    // Arrange
    var company = new Company(12345, "Test Company");

    // Act
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    company.AddIncome(2020, 2000000m, "10-K", "CY2020");

    // Assert
    company.Incomes.Should().HaveCount(3);
  }

  [Fact]
  public void GetIncomeForYear_WithExistingYear_ShouldReturnIncome()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2021, 1000000m, "10-K", "CY2021");

    // Act
    var income = company.GetIncomeForYear(2021);

    // Assert
    income.Should().Be(1000000m);
  }

  [Fact]
  public void GetIncomeForYear_WithNonExistingYear_ShouldReturnZero()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2021, 1000000m, "10-K", "CY2021");

    // Act
    var income = company.GetIncomeForYear(2020);

    // Assert
    income.Should().Be(0);
  }

  [Fact]
  public void HasCompleteDataForYears_WithAllYearsPresent_ShouldReturnTrue()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    company.AddIncome(2020, 2000000m, "10-K", "CY2020");
    company.AddIncome(2021, 2500000m, "10-K", "CY2021");
    company.AddIncome(2022, 3000000m, "10-K", "CY2022");

    // Act
    var result = company.HasCompleteDataForYears(2018, 2022);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void HasCompleteDataForYears_WithMissingYear_ShouldReturnFalse()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2018, 1000000m, "10-K", "CY2018");
    company.AddIncome(2019, 1500000m, "10-K", "CY2019");
    // Missing 2020
    company.AddIncome(2021, 2500000m, "10-K", "CY2021");
    company.AddIncome(2022, 3000000m, "10-K", "CY2022");

    // Act
    var result = company.HasCompleteDataForYears(2018, 2022);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void HasCompleteDataForYears_WithNoData_ShouldReturnFalse()
  {
    // Arrange
    var company = new Company(12345, "Test Company");

    // Act
    var result = company.HasCompleteDataForYears(2018, 2022);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void HasCompleteDataForYears_WithSingleYear_ShouldWorkCorrectly()
  {
    // Arrange
    var company = new Company(12345, "Test Company");
    company.AddIncome(2021, 1000000m, "10-K", "CY2021");

    // Act
    var result = company.HasCompleteDataForYears(2021, 2021);

    // Assert
    result.Should().BeTrue();
  }
}
