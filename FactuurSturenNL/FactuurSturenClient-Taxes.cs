using System.Text.Json;
using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private List<Tax>? _cachedTaxes;

  public async Task<IEnumerable<Tax>> GetTaxesAsync(bool? allowCache = null)
  {
    allowCache ??= _allowResponseCaching;

    if (allowCache.Value && _cachedTaxes != null)
      return _cachedTaxes;

    HttpResponseMessage? response = await HttpClient.GetAsync("taxes");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get taxes");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get taxes");
    Tax[]? result = JsonSerializer.Deserialize<Tax[]>(responseBody, _jsonSerializerOptions);
    if (result == null) throw new HttpRequestException("Failed to deserialize taxes");

    if (allowCache.Value)
      _cachedTaxes = new List<Tax>(result);

    return result;
  }

  public async Task<Tax?> GetTaxTypeByRateAsync(TaxRate taxRate, bool? allowCache = null)
  {
    return (await GetTaxesAsync(allowCache)).FirstOrDefault(t => t.Type == taxRate);
  }
}