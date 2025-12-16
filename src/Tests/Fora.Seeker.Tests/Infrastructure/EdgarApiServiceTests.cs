using System.Net;
using System.Text.Json;
using Fora.Seeker.Web.Infrastructure.Edgar;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Fora.Seeker.Tests.Infrastructure;

public class EdgarApiServiceTests
{
  private readonly ILogger<EdgarApiService> _logger;
  private readonly HttpMessageHandler _httpMessageHandler;

  public EdgarApiServiceTests()
  {
    _logger = Substitute.For<ILogger<EdgarApiService>>();
    _httpMessageHandler = Substitute.For<HttpMessageHandler>();
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithValidCik_ShouldReturnCompanyInfo()
  {
    // Arrange
    var cik = 12345;
    var expectedCompanyInfo = new EdgarCompanyInfo
    {
      Cik = cik,
      EntityName = "Test Company",
      Facts = new EdgarCompanyInfo.InfoFact
      {
        UsGaap = new EdgarCompanyInfo.InfoFactUsGaap
        {
          NetIncomeLoss = new EdgarCompanyInfo.InfoFactUsGaapNetIncomeLoss
          {
            Units = new EdgarCompanyInfo.InfoFactUsGaapIncomeLossUnits
            {
              Usd = new[]
              {
                new EdgarCompanyInfo.InfoFactUsGaapIncomeLossUnitsUsd
                {
                  Form = "10-K",
                  Frame = "CY2021",
                  Val = 1000000m
                }
              }
            }
          }
        }
      }
    };

    var responseContent = JsonSerializer.Serialize(expectedCompanyInfo);
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, responseContent);
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    var result = await service.GetCompanyFactsAsync(cik);

    // Assert
    result.Should().NotBeNull();
    result!.Cik.Should().Be(cik);
    result.EntityName.Should().Be("Test Company");
  }

  [Fact]
  public async Task GetCompanyFactsAsync_ShouldFormatCikWithLeadingZeros()
  {
    // Arrange
    var cik = 12345;
    var expectedUrl = "https://data.sec.gov/api/xbrl/companyfacts/CIK0000012345.json";
    
    var responseContent = JsonSerializer.Serialize(new EdgarCompanyInfo
    {
      Cik = cik,
      EntityName = "Test Company"
    });

    HttpRequestMessage? capturedRequest = null;
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, responseContent, req =>
    {
      capturedRequest = req;
    });

    var service = new EdgarApiService(httpClient, _logger);

    // Act
    await service.GetCompanyFactsAsync(cik);

    // Assert
    capturedRequest.Should().NotBeNull();
    capturedRequest!.RequestUri!.ToString().Should().Be(expectedUrl);
  }

  [Fact]
  public async Task GetCompanyFactsAsync_ShouldSetCorrectHeaders()
  {
    // Arrange
    var cik = 12345;
    var responseContent = JsonSerializer.Serialize(new EdgarCompanyInfo
    {
      Cik = cik,
      EntityName = "Test Company"
    });

    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, responseContent);
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    await service.GetCompanyFactsAsync(cik);

    // Assert
    httpClient.DefaultRequestHeaders.UserAgent.ToString().Should().Contain("PostmanRuntime/7.34.0");
    httpClient.DefaultRequestHeaders.Accept.ToString().Should().Contain("*/*");
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithHttpError_ShouldReturnNull()
  {
    // Arrange
    var cik = 12345;
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.NotFound, "");
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    var result = await service.GetCompanyFactsAsync(cik);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithInvalidJson_ShouldReturnNull()
  {
    // Arrange
    var cik = 12345;
    var invalidJson = "{ invalid json content }";
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, invalidJson);
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    var result = await service.GetCompanyFactsAsync(cik);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithEmptyResponse_ShouldReturnNull()
  {
    // Arrange
    var cik = 12345;
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, "");
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    var result = await service.GetCompanyFactsAsync(cik);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithServerError_ShouldReturnNull()
  {
    // Arrange
    var cik = 12345;
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.InternalServerError, "");
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    var result = await service.GetCompanyFactsAsync(cik);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithCancellationToken_ShouldPassTokenToHttpClient()
  {
    // Arrange
    var cik = 12345;
    var responseContent = JsonSerializer.Serialize(new EdgarCompanyInfo
    {
      Cik = cik,
      EntityName = "Test Company"
    });

    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, responseContent);
    var service = new EdgarApiService(httpClient, _logger);
    var cancellationToken = new CancellationToken();

    // Act
    var result = await service.GetCompanyFactsAsync(cik, cancellationToken);

    // Assert
    result.Should().NotBeNull();
  }

  [Fact]
  public async Task GetCompanyFactsAsync_WithComplexNestedData_ShouldDeserializeCorrectly()
  {
    // Arrange
    var cik = 12345;
    var expectedCompanyInfo = new EdgarCompanyInfo
    {
      Cik = cik,
      EntityName = "Complex Company",
      Facts = new EdgarCompanyInfo.InfoFact
      {
        UsGaap = new EdgarCompanyInfo.InfoFactUsGaap
        {
          NetIncomeLoss = new EdgarCompanyInfo.InfoFactUsGaapNetIncomeLoss
          {
            Units = new EdgarCompanyInfo.InfoFactUsGaapIncomeLossUnits
            {
              Usd = new[]
              {
                new EdgarCompanyInfo.InfoFactUsGaapIncomeLossUnitsUsd
                {
                  Form = "10-K",
                  Frame = "CY2021",
                  Val = 1000000m
                },
                new EdgarCompanyInfo.InfoFactUsGaapIncomeLossUnitsUsd
                {
                  Form = "10-K",
                  Frame = "CY2022",
                  Val = 2000000m
                }
              }
            }
          }
        }
      }
    };

    var responseContent = JsonSerializer.Serialize(expectedCompanyInfo);
    var httpClient = CreateHttpClientWithMockedResponse(HttpStatusCode.OK, responseContent);
    var service = new EdgarApiService(httpClient, _logger);

    // Act
    var result = await service.GetCompanyFactsAsync(cik);

    // Assert
    result.Should().NotBeNull();
    result!.Facts.UsGaap.Should().NotBeNull();
    result.Facts.UsGaap!.NetIncomeLoss.Should().NotBeNull();
    result.Facts.UsGaap.NetIncomeLoss!.Units.Should().NotBeNull();
    result.Facts.UsGaap.NetIncomeLoss.Units!.Usd.Should().HaveCount(2);
  }

  private HttpClient CreateHttpClientWithMockedResponse(
    HttpStatusCode statusCode,
    string content,
    Action<HttpRequestMessage>? requestCallback = null)
  {
    var handler = new MockHttpMessageHandler(statusCode, content, requestCallback);
    return new HttpClient(handler);
  }

  private class MockHttpMessageHandler : HttpMessageHandler
  {
    private readonly HttpStatusCode _statusCode;
    private readonly string _content;
    private readonly Action<HttpRequestMessage>? _requestCallback;

    public MockHttpMessageHandler(
      HttpStatusCode statusCode,
      string content,
      Action<HttpRequestMessage>? requestCallback = null)
    {
      _statusCode = statusCode;
      _content = content;
      _requestCallback = requestCallback;
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      _requestCallback?.Invoke(request);

      var response = new HttpResponseMessage(_statusCode)
      {
        Content = new StringContent(_content)
      };

      return Task.FromResult(response);
    }
  }
}
