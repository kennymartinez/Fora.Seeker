using FastEndpoints;
using Fora.Seeker.Web.Domain.CompanyAggregate;
using Fora.Seeker.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fora.Seeker.Web.CompanyFeatures;

public class GetCompaniesRequest
{
  public string? StartsWithLetter { get; set; }
}

public class GetCompanies : Endpoint<GetCompaniesRequest, List<CompanyResponse>>
{
  private readonly AppDbContext _dbContext;
  private readonly ILogger<GetCompanies> _logger;

  public GetCompanies(AppDbContext dbContext, ILogger<GetCompanies> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  public override void Configure()
  {
    Get("/Companies");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetCompaniesRequest req, CancellationToken ct)
  {
    var query = _dbContext.Companies
      .Include(c => c.Incomes)
      .AsQueryable();

    // Apply name filter if provided
    if (!string.IsNullOrWhiteSpace(req.StartsWithLetter))
    {
      var letter = req.StartsWithLetter.Trim().ToUpperInvariant();
      if (letter.Length > 0)
      {
        query = query.Where(c => c.Name.ToUpper().StartsWith(letter));
      }
    }

    var companies = await query.ToListAsync(ct);

    var response = companies.Select(company => new CompanyResponse
    {
      Id = company.Id,
      Name = company.Name,
      StandardFundableAmount = Math.Round(FundingCalculator.CalculateStandardFundableAmount(company), 2),
      SpecialFundableAmount = Math.Round(FundingCalculator.CalculateSpecialFundableAmount(company), 2)
    }).ToList();

    _logger.LogInformation("Returning {Count} companies", response.Count);

    Response = response;
  }
}
