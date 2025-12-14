namespace Fora.Seeker.Web.CompanyFeatures;

public class CompanyResponse
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal StandardFundableAmount { get; set; }
  public decimal SpecialFundableAmount { get; set; }
}
