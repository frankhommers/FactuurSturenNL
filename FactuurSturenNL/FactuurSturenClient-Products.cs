using System.Text;
using System.Text.Json;
using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private List<Product>? _cachedProducts;

  public async Task<IEnumerable<Product>> GetProductsAsync(bool? allowCache = null)
  {
    allowCache ??= _allowResponseCaching;

    if (allowCache.Value && _cachedProducts != null)
      return _cachedProducts.ToArray();

    HttpResponseMessage? response = await HttpClient.GetAsync("products");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get products");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get products");
    Product[]? result = JsonSerializer.Deserialize<Product[]>(responseBody, _jsonSerializerOptions);
    if (result == null) throw new HttpRequestException("Failed to deserialize products");
    if (allowCache.Value)
      _cachedProducts = new List<Product>(result);

    return result;
  }

  public async Task<Product> GetProductAsync(int id, bool? allowCache = null)
  {
    allowCache ??= _allowResponseCaching;
    Product? result = null;

    if (allowCache.Value)
    {
      result = _cachedProducts?.FirstOrDefault(p => p.Id == id);
      if (result != null) return result;
    }

    HttpResponseMessage? response = await HttpClient.GetAsync($"products/{id}");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get product");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get product");
    result = JsonSerializer.Deserialize<Product>(responseBody, _jsonSerializerOptions);
    if (result == null) throw new HttpRequestException("Failed to deserialize product");
    if (allowCache.Value)
      StoreInProductCache(result);

    return result;
  }

  public async Task<Product> CreateProductAsync(Product product, bool? storeInCache = null)
  {
    storeInCache ??= _allowResponseCaching;

    StringContent jsonContent = new(JsonSerializer.Serialize(product, _jsonSerializerOptions), Encoding.UTF8,
      "application/json");

    HttpResponseMessage response = await HttpClient.PostAsync("products", jsonContent);

    response.EnsureSuccessStatusCode();

    string? responseString = await response.Content.ReadAsStringAsync();
    product.Id = Convert.ToInt32(responseString);

    if (storeInCache.Value)
      StoreInProductCache(product);

    // Assuming StoreInCache updates the _cachedProducts internally
    return product;
  }


  public async Task<Product> UpdateProductAsync(Product product, bool? storeInCache = null)
  {
    storeInCache ??= _allowResponseCaching;

    StringContent jsonContent = new(JsonSerializer.Serialize(product, _jsonSerializerOptions), Encoding.UTF8,
      "application/json");
    HttpResponseMessage? response = await HttpClient.PutAsync($"products/{product.Id}", jsonContent);
    response.EnsureSuccessStatusCode();
    if (storeInCache.Value)
      StoreInProductCache(product);

    return product;
  }

  public async Task DeleteProductAsync(Product product, bool? storeInCache = null)
  {
    if (product == null)
      throw new ArgumentNullException(nameof(product));
    int productId = product.Id;
    await DeleteProductAsync(productId, storeInCache);
  }

  public async Task DeleteProductAsync(int productId, bool? storeInCache = null)
  {
    storeInCache ??= _allowResponseCaching;
    HttpResponseMessage? response = await HttpClient.DeleteAsync($"products/{productId}");
    response.EnsureSuccessStatusCode();
    if (storeInCache.Value) RemoveFromProductCache(productId);
  }

  private void StoreInProductCache(Product product)
  {
    _cachedProducts ??= new List<Product>();
    _cachedProducts.Add(product);
  }


  private void RemoveFromProductCache(Product? product)
  {
    if (product == null)
      throw new ArgumentNullException(nameof(product));

    int productId = product.Id;
    RemoveFromProductCache(productId);
  }

  private void RemoveFromProductCache(int productId)
  {
    if (_cachedProducts == null)
      return;
    if (_cachedProducts.Any(p => p.Id == productId))
      _cachedProducts.Remove(_cachedProducts.First(p => p.Id == productId));
  }
}