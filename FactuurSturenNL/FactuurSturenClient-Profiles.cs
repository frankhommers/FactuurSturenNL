using System.Text.Json;
using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private List<Profile>? _cachedProfiles;

  public async Task<IEnumerable<Profile>> GetProfilesAsync(bool? allowCache = null)
  {
    allowCache ??= _allowResponseCaching;

    if (allowCache.Value && _cachedProfiles != null)
      return _cachedProfiles;

    HttpResponseMessage? response = await HttpClient.GetAsync("profiles");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get profiles");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get profiles");
    Profile[]? result = JsonSerializer.Deserialize<Profile[]>(responseBody, _jsonSerializerOptions);
    if (result == null) throw new HttpRequestException("Failed to deserialize profiles");
    if (allowCache.Value)
      _cachedProfiles = new List<Profile>(result);

    return result!;
  }
}