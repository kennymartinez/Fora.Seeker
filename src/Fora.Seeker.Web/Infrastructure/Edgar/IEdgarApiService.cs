namespace Fora.Seeker.Web.Infrastructure.Edgar;

public interface IEdgarApiService
{
  Task<EdgarCompanyInfo?> GetCompanyFactsAsync(int cik, CancellationToken cancellationToken = default);
}
