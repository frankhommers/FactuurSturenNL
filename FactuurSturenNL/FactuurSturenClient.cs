using System.Collections;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using FactuurSturenNL.Exceptions;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private readonly bool _allowResponseCaching;
  private readonly string _apiUrl = "https://www.factuursturen.nl/api/v1/";

  private readonly JsonSerializerOptions _jsonSerializerOptions = new()
  {
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
  };
  
  internal readonly bool UsePollyTransientFaultHandling;

  private HttpClient? _httpClient;
  private HttpClient HttpClient
  {
    get
    {
      if (!_initialized || _httpClient == null)
        throw new TypeInitializationException("FactuurSturenClient", new Exception("Not initialized"));
      return _httpClient!;
    }
  }

  private bool _initialized;


  /// <summary>
  ///   Initializes a new FactuurSturenClient instance with a custom API url.
  /// </summary>
  /// <param name="apiUrl">Endpoint address of the FactuurSturen.nl REST API</param>
  /// <param name="usePollyTransientFaultHandling">Whether or not to use transient fault handling</param>
  /// <param name="allowResponseCaching">Whether or not to allow response caching</param>
  public FactuurSturenClient(string? apiUrl = null, bool usePollyTransientFaultHandling = true,
    bool allowResponseCaching = true)
  {
    if (apiUrl != null)
      _apiUrl = apiUrl;

    UsePollyTransientFaultHandling = usePollyTransientFaultHandling;
    _allowResponseCaching = allowResponseCaching;
  }

  /// <summary>
  ///   Authenticate this client with the FactuurSturen API.
  /// </summary>
  /// <param name="userName">User name (typically the email address)</param>
  /// <param name="apiKey">Password</param>
  public async Task LoginAsync(string userName, string apiKey)
  {
    _httpClient = new HttpClient
    {
      BaseAddress = new Uri(_apiUrl)
    };
    _httpClient.DefaultRequestHeaders.Clear();
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
      Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{apiKey}")));
    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("factuursturen.nl-dotnet-lib/1.0");

    HttpRequestMessage request = new(HttpMethod.Get, "countrylist/en");
    HttpResponseMessage? response = await _httpClient.SendAsync(request);
    if (response.IsSuccessStatusCode)
      _initialized = true;
    else
      throw new AuthenticationException("API didn't return success code");
  }

}
