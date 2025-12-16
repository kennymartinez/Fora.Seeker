using Fora.Seeker.Web.Domain.CompanyAggregate;

namespace Fora.Seeker.Tests.Domain;

public class CompanyIncomeTests
{
  [Fact]
  public void Constructor_WithValidParameters_ShouldCreateCompanyIncome()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    var form = "10-K";
    var frame = "CY2021";

    // Act
    var income = new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    income.Year.Should().Be(year);
    income.IncomeAmount.Should().Be(incomeAmount);
    income.Form.Should().Be(form);
    income.Frame.Should().Be(frame);
  }

  [Fact]
  public void Constructor_WithNegativeIncome_ShouldAllowNegativeValue()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = -500000m;
    var form = "10-K";
    var frame = "CY2021";

    // Act
    var income = new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    income.IncomeAmount.Should().Be(incomeAmount);
  }

  [Fact]
  public void Constructor_WithZeroYear_ShouldThrowException()
  {
    // Arrange
    var year = 0;
    var incomeAmount = 1000000m;
    var form = "10-K";
    var frame = "CY2021";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithNegativeYear_ShouldThrowException()
  {
    // Arrange
    var year = -2021;
    var incomeAmount = 1000000m;
    var form = "10-K";
    var frame = "CY2021";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithNullForm_ShouldThrowException()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    string form = null!;
    var frame = "CY2021";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithEmptyForm_ShouldThrowException()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    var form = "";
    var frame = "CY2021";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithWhitespaceForm_ShouldThrowException()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    var form = "   ";
    var frame = "CY2021";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithNullFrame_ShouldThrowException()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    var form = "10-K";
    string frame = null!;

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithEmptyFrame_ShouldThrowException()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    var form = "10-K";
    var frame = "";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_WithWhitespaceFrame_ShouldThrowException()
  {
    // Arrange
    var year = 2021;
    var incomeAmount = 1000000m;
    var form = "10-K";
    var frame = "   ";

    // Act
    var act = () => new CompanyIncome(year, incomeAmount, form, frame);

    // Assert
    act.Should().Throw<ArgumentException>();
  }
}
