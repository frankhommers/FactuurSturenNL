using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private const string ResourceInvoicesRepeating = "invoices_repeated";
  private readonly List<Invoice>? _cachedRepeatingInvoices;

  /// <summary>
  ///   Returns a list of repeating invoices.
  /// </summary>
  /// <param name="allowCache"></param>
  /// <returns></returns>
  public async Task<IEnumerable<Invoice>> GetRepeatingInvoices(bool? allowCache = null)
  {
    return await GetInvoicesInternalAsync(ResourceInvoicesRepeating, allowCache, _cachedRepeatingInvoices);
  }

  /// <summary>
  ///   Return a specific repeating invoice
  /// </summary>
  /// <param name="invoiceNr">Invoice number</param>
  /// <param name="allowCache">Whether or not to look it up/store it in cache</param>
  /// <returns></returns>
  public async Task<Invoice> GetRepeatingInvoiceAsync(string invoiceNr, bool allowCache = true)
  {
    return await GetInvoiceInternalAsync(ResourceInvoicesRepeating, invoiceNr, allowCache, _cachedRepeatingInvoices);
  }

  /// <summary>
  ///   Delete a repeating invoice.
  /// </summary>
  /// <param name="invoice"></param>
  /// <returns></returns>
  public async Task DeleteRepeatingInvoiceAsync(Invoice invoice)
  {
    if (invoice == null)
      throw new ArgumentNullException(nameof(invoice));
    await DeleteInvoiceInternalAsync(ResourceInvoicesRepeating, invoice.InvoiceNr, _cachedRepeatingInvoices);
  }

  /// <summary>
  ///   Delete a repeating invoice.
  /// </summary>
  /// <param name="invoiceNr"></param>
  /// <returns></returns>
  public async Task DeleteRepeatingInvoiceAsync(string invoiceNr)
  {
    await DeleteInvoiceInternalAsync(ResourceInvoicesRepeating, invoiceNr, _cachedRepeatingInvoices);
  }
}