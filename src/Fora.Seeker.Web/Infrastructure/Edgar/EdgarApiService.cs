using System.Text.Json;

namespace Fora.Seeker.Web.Infrastructure.Edgar;

public class EdgarApiService : IEdgarApiService
{
  private readonly HttpClient _httpClient;
  private readonly ILogger<EdgarApiService> _logger;
  private const string BaseUrl = "https://data.sec.gov/api/xbrl/companyfacts/";

  public EdgarApiService(HttpClient httpClient, ILogger<EdgarApiService> logger)
  {
    _httpClient = httpClient;
    _logger = logger;

    // Configure headers as specified in requirements
    _httpClient.DefaultRequestHeaders.Clear();
    _httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.34.0");
    _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
  }

  public async Task<EdgarCompanyInfo?> GetCompanyFactsAsync(int cik, CancellationToken cancellationToken = default)
  {
    try
    {
      // Format CIK with leading zeros (10 digits)
      string formattedCik = cik.ToString("D10");
      string url = $"{BaseUrl}CIK{formattedCik}.json";

      _logger.LogInformation("Fetching company facts for CIK {Cik} from {Url}", cik, url);

      var response = await _httpClient.GetAsync(url, cancellationToken);

      if (!response.IsSuccessStatusCode)
      {
        _logger.LogWarning("Failed to fetch data for CIK {Cik}. Status: {StatusCode}", cik, response.StatusCode);
        return null;
      }

      var content = await response.Content.ReadAsStringAsync(cancellationToken);
      
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      var companyInfo = JsonSerializer.Deserialize<EdgarCompanyInfo>(content, options);

      if (companyInfo != null)
      {
        _logger.LogInformation("Successfully fetched data for CIK {Cik} - {EntityName}", cik, companyInfo.EntityName);
      }

      return companyInfo;
    }
    catch (HttpRequestException ex)
    {
      _logger.LogError(ex, "HTTP error fetching data for CIK {Cik}", cik);
      return null;
    }
    catch (JsonException ex)
    {
      _logger.LogError(ex, "JSON parsing error for CIK {Cik}", cik);
      return null;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unexpected error fetching data for CIK {Cik}", cik);
      return null;
    }
  }
}
