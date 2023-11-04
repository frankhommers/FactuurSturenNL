using System.Text;
using System.Text.Json;
using FactuurSturenNL.Exceptions;
using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private const string ResourceInvoices = "invoices";
  private List<Invoice>? _cachedInvoices;

  /// <summary>
  ///   Returns a list of sent invoices.
  /// </summary>
  /// <param name="allowCache"></param>
  /// <returns></returns>
  public async Task<IEnumerable<Invoice>> GetInvoicesAsync(bool? allowCache = null)
  {
    return await GetInvoicesInternalAsync(ResourceInvoices, allowCache, _cachedInvoices);
  }

  private async Task<IEnumerable<Invoice>> GetInvoicesInternalAsync(string resource, bool? allowCache,
    List<Invoice>? cachedInvoices)
  {
    allowCache ??= _allowResponseCaching;

    if (allowCache.Value && cachedInvoices != null && cachedInvoices.Count > 0)
      return cachedInvoices;

    HttpResponseMessage? response = await HttpClient.GetAsync(resource);

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get invoices");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get invoices");

    Invoice[]? result = JsonSerializer.Deserialize<Invoice[]>(responseBody, _jsonSerializerOptions);
    if (result == null) throw new HttpRequestException("Failed to deserialize invoices");

    if (allowCache.Value)
      cachedInvoices?.AddRange(result);

    return result;
  }

  /// <summary>
  ///   Returns a list of sent invoices. Filter by an invoice filter.
  /// </summary>
  /// <param name="filter"></param>
  /// <returns></returns>
  public async Task<IEnumerable<Invoice>> GetInvoicesWithFilterAsync(InvoiceFilters filter)
  {
    HttpResponseMessage? response =
      await HttpClient.GetAsync(
        $"{ResourceInvoices}?filter={Enum.GetName(typeof(InvoiceFilters), filter).ToLowerInvariant()}");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get invoices");

    string? responseBody = await response.Content.ReadAsStringAsync();
    if (responseBody == null) throw new HttpRequestException("Failed to get invoices");
    Invoice[]? result = JsonSerializer.Deserialize<Invoice[]>(responseBody, _jsonSerializerOptions);
    if (result == null) throw new HttpRequestException("Failed to deserialize invoices");

    return result;
  }

  /// <summary>
  ///   Return a specific sent invoice.
  /// </summary>
  /// <param name="invoiceNr">Invoice number</param>
  /// <param name="allowCache">Whether or not to look it up/store it in cache</param>
  /// <returns></returns>
  public async Task<Invoice> GetInvoiceAsync(string invoiceNr, bool? allowCache = null)
  {
    return await GetInvoiceInternalAsync(ResourceInvoices, invoiceNr, allowCache, _cachedInvoices);
  }

  /// <summary>
  ///   Returns the downloaded PDF version of an invoice.
  /// </summary>
  /// <param name="invoiceNr">Invoice number</param>
  /// <returns></returns>
  public async Task<byte[]> GetInvoicePdfAsync(string invoiceNr)
  {
    HttpResponseMessage? response = await HttpClient.GetAsync($"invoices_pdf/{invoiceNr}");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get invoice PDF");

    return await response.Content.ReadAsByteArrayAsync();
  }

  private async Task<Invoice?> GetInvoiceInternalAsync(string resource, string invoiceNr, bool? allowCache,
    List<Invoice> cachedInvoices)
  {
    Invoice? result = null;
    allowCache ??= _allowResponseCaching;

    if (allowCache.Value)
    {
      result = _cachedInvoices?.FirstOrDefault(p => p.InvoiceNr == invoiceNr);
      if (result != null) return result;
    }

    HttpResponseMessage? response = await HttpClient.GetAsync($"{resource}/{invoiceNr}");

    if (!response.IsSuccessStatusCode)
      throw new HttpRequestException("Failed to get the invoice");

    string? responseString = await response.Content.ReadAsStringAsync();
    result = JsonSerializer.Deserialize<InvoiceContainer>(responseString, _jsonSerializerOptions)?.Invoice;

    if (result != null && allowCache.Value)
      StoreInvoiceInCache(result, cachedInvoices);

    return result;
  }

  /// <summary>
  ///   Creates an invoice. Do not call this method if you want to create a draft invoice.
  /// </summary>
  /// <param name="invoice">The invoice to create</param>
  /// <param name="returnReloadedInvoice">Whether or not to fetch updated invoice data</param>
  /// <param name="storeReloadedVersionInCache"></param>
  /// <returns>An updated Invoice record (retrieved from the server) when returnReloadedInvoice is True, else Null. </returns>
  public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, bool returnReloadedInvoice,
    bool? storeReloadedVersionInCache = true)
  {
    if (invoice.Action == InvoiceAction.Save)
      throw new InvalidOperationException(
        "The Action cannot be 'Save' when calling this method. Call CreateDraftInvoice instead.");

    ValidateInvoice(invoice);

    StringContent jsonContent = new(JsonSerializer.Serialize(invoice, _jsonSerializerOptions), Encoding.UTF8,
      "application/json");
    HttpResponseMessage? response = await HttpClient.PostAsync($"{ResourceInvoices}/", jsonContent);

    response.EnsureSuccessStatusCode();
    string? responseString = await response.Content.ReadAsStringAsync();
    invoice.InvoiceNr = responseString;

    if (returnReloadedInvoice)
    {
      invoice = await GetInvoiceAsync(responseString, storeReloadedVersionInCache);
      return invoice;
    }

    return invoice;
  }

  /// <summary>
  ///   Creates an invoice and save is as a draft. This invoice cannot be retrieved via GetInvoices and cannot
  ///   be send later on. To send a draft invoice, you must go to the web application.
  /// </summary>
  /// <param name="invoice"></param>
  /// <returns></returns>
  public async Task CreateDraftInvoiceAsync(Invoice invoice)
  {
    if (invoice.Action != InvoiceAction.Save)
      throw new InvalidOperationException("The Action must be set to Save when calling this method.");

    ValidateInvoice(invoice);

    string invoiceString = JsonSerializer.Serialize(invoice, _jsonSerializerOptions);
    StringContent jsonContent = new(invoiceString, Encoding.UTF8,
      "application/json");
    HttpResponseMessage? response = await HttpClient.PostAsync($"{ResourceInvoices}/", jsonContent);
    if (!response.IsSuccessStatusCode)
    {
      string content = await response.Content.ReadAsStringAsync();
      throw new DetailedHttpRequestException($"Failed to create draft invoice")
      {
        Response = content,
        Request = invoiceString
      };
    }
  }

  private static void ValidateInvoice(Invoice invoice)
  {
    if (invoice.Action == InvoiceAction.None)
      throw new InvalidOperationException("When creating an invoice, the action must be set.");
    if (invoice.Action == InvoiceAction.Send && invoice.SendMethod == SendMethod.None)
      throw new InvalidOperationException("When the invoice action is Send, the SendMethod must be set.");
    if ((invoice.Action == InvoiceAction.Save || invoice.Action == InvoiceAction.Repeat) &&
        string.IsNullOrWhiteSpace(invoice.SaveName))
      throw new InvalidOperationException(
        "When the action is 'save' or 'repeat' you must supply a SaveName.");
    if (invoice.Action == InvoiceAction.Repeat)
    {
      if (!invoice.InitialDate.HasValue)
        throw new InvalidOperationException(
          "Because the action is 'repeat', InitialDate must be set. Is the date when the first invoice must be sent");
      if (!invoice.FinalSendDate.HasValue)
        throw new InvalidOperationException(
          "Because the action is 'repeat', FinalSendDate must be set. Is the date when the last invoice must be sent. After this date the recurring invoice entry is deleted");
      if (invoice.Frequency == Frequency.None)
        throw new InvalidOperationException(
          "Because the action is 'repeat', the Frequency must be set. Is the frequency when the invoice must be sent. Based on the InitialDate.");
      if (invoice.RepeatType == RepeatType.None)
        throw new InvalidOperationException(
          "Because the action is 'repeat', the RepeatType must be set. Set if the recurring invoice is automatically sent by our system");
    }
  }

  public async Task DeleteInvoiceAsync(Invoice invoice)
  {
    if (invoice == null)
      throw new ArgumentNullException(nameof(invoice));
    string invoiceNr = invoice.InvoiceNr;
    await DeleteInvoiceAsync(invoiceNr);
  }

  public async Task DeleteInvoiceAsync(string invoiceNr)
  {
    await DeleteInvoiceInternalAsync(ResourceInvoices, invoiceNr, _cachedInvoices);
  }

  private async Task DeleteInvoiceInternalAsync(string resource, string invoiceNr, List<Invoice> cacheList)
  {
    HttpResponseMessage? response = await HttpClient.DeleteAsync($"{resource}/{invoiceNr}");

    response.EnsureSuccessStatusCode();

    RemoveInvoiceFromCache(invoiceNr, cacheList);
  }

  private void StoreInvoiceInCache(Invoice invoice, List<Invoice>? invoiceCache)
  {
    if (invoiceCache == null) return;
    invoiceCache?.Add(invoice);
  }

  private void RemoveInvoiceFromCache(string invoiceId, List<Invoice>? invoiceCache)
  {
    if (invoiceCache == null) return;
    if (invoiceCache.Any(p => p.Id == invoiceId))
      invoiceCache.Remove(invoiceCache.First(p => p.Id == invoiceId));
  }
}