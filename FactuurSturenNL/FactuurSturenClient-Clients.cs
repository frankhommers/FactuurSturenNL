using System.Text;
using System.Text.Json;
using FactuurSturenNL.Exceptions;
using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private List<Client>? _cachedClients;

  public async Task<IEnumerable<Client>> GetClientsAsync(bool? allowCache = null)
  {
    allowCache ??= _allowResponseCaching;

    if (allowCache.Value && _cachedClients != null)
      return _cachedClients;

    HttpResponseMessage? response = await HttpClient.GetAsync("clients");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get clients");

    string? responseBody = await response.Content.ReadAsStringAsync();
    Client[]? result = JsonSerializer.Deserialize<Client[]>(responseBody, _jsonSerializerOptions);

    if (result == null)
      throw new HttpRequestException("Failed to deserialize clients");

    if (allowCache.Value)
      _cachedClients = new List<Client>(result);

    return result;
  }

  public async Task<Client> GetClientAsync(int clientNr, bool? allowCache = null)
  {
    allowCache ??= _allowResponseCaching;
    Client? result = null;
    if (allowCache.Value)
    {
      result = _cachedClients?.FirstOrDefault(c => c.ClientNr == clientNr);
      if (result != null) return result;
    }

    HttpResponseMessage? response = await HttpClient.GetAsync($"clients/{clientNr}");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get client");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get client");
    result = JsonSerializer.Deserialize<ClientContainer>(responseBody, _jsonSerializerOptions)?.Client;
    if (result == null) throw new HttpRequestException("Failed to deserialize client");

    if (allowCache.Value)
      StoreClientInCache(result);

    return result;
  }

  /// <summary>
  ///   Gets a client by company name. Finds the client by searching through ALL clients.
  /// </summary>
  /// <param name="companyName">Company name</param>
  /// <param name="allowCache">Whether or not to allow cache</param>
  /// <returns></returns>
  public async Task<Client?> GetClientAsync(string companyName, bool? allowCache = null)
  {
    if (companyName == null) throw new ArgumentNullException(nameof(companyName));

    return (await GetClientsAsync(allowCache)).FirstOrDefault(
      c => companyName.Equals(c.Company, StringComparison.CurrentCultureIgnoreCase));
  }

  public async Task<Client> CreateClientAsync(Client client, bool? storeInCache = null)
  {
    storeInCache ??= _allowResponseCaching;

    StringContent jsonContent = new(JsonSerializer.Serialize(client, _jsonSerializerOptions), Encoding.UTF8,
      "application/json");
    HttpResponseMessage? response = await HttpClient.PostAsync("clients", jsonContent);

    if (response.IsSuccessStatusCode)
    {
      string? responseString = await response.Content.ReadAsStringAsync();
      client.ClientNr = Convert.ToInt32(responseString);
      if (storeInCache.Value)
        StoreClientInCache(client);
      return client;
    }

    throw new HttpRequestException("Failed to create client");
  }

  public async Task<Client> UpdateClientAsync(Client client, bool? storeInCache = null)
  {
    if (client == null) throw new ArgumentNullException(nameof(client));
    storeInCache ??= _allowResponseCaching;

    StringContent jsonContent = new(JsonSerializer.Serialize(client, _jsonSerializerOptions), Encoding.UTF8,
      "application/json");
    HttpResponseMessage? response = await HttpClient.PutAsync($"clients/{client.ClientNr}", jsonContent);

    if (response.IsSuccessStatusCode)
    {
      if (storeInCache.Value)
        StoreClientInCache(client);

      return client;
    }

    throw new HttpRequestException("Failed to create client");
  }

  public async Task DeleteClientAsync(Client client)
  {
    if (client == null)
      throw new ArgumentNullException(nameof(client));
    int clientNr = client.ClientNr;
    await DeleteClientAsync(clientNr);
  }

  public async Task DeleteClientAsync(int clientNr)
  {
    HttpResponseMessage? response = await HttpClient.DeleteAsync($"clients/{clientNr}");

    response.EnsureSuccessStatusCode();

    RemoveClientFromCache(clientNr);
  }

  private void StoreClientInCache(Client client)
  {
    if (_cachedClients == null)
      _cachedClients = new List<Client>();
    _cachedClients.Add(client);
  }

  private void RemoveClientFromCache(Client client)
  {
    if (client == null)
      throw new ArgumentNullException(nameof(client));

    int clientNr = client.ClientNr;
    RemoveClientFromCache(clientNr);
  }

  private void RemoveClientFromCache(int clientNr)
  {
    if (_cachedClients == null) return;
    if (_cachedClients.Any(p => p.ClientNr == clientNr))
      _cachedClients.Remove(_cachedClients.First(p => p.ClientNr == clientNr));
  }
}