using FactuurSturenNL.Model;

namespace FactuurSturenNL;

public partial class FactuurSturenClient
{
  private const string ResourceInvoicesSaved = "invoices_saved";
  private readonly List<Invoice> _cachedSavedInvoices = new();

  /// <summary>
  ///   Returns a list of saved invoices.
  /// </summary>
  /// <param name="allowCache"></param>
  /// <returns></returns>
  public async Task<IEnumerable<Invoice>> GetSavedInvoicesAsync(bool? allowCache = null)
  {
    return await GetInvoicesInternalAsync(ResourceInvoicesSaved, allowCache, _cachedSavedInvoices);
  }

  /// <summary>
  ///   Return a specific saved invoice
  /// </summary>
  /// <param name="invoiceNr">Invoice number</param>
  /// <param name="allowCache">Whether or not to look it up/store it in cache</param>
  /// <returns></returns>
  public async Task<Invoice> GetSavedInvoice(string invoiceNr, bool allowCache = true)
  {
    return await GetInvoiceInternalAsync(ResourceInvoicesSaved, invoiceNr, allowCache, _cachedSavedInvoices);
  }

  /// <summary>
  ///   Delete a saved invoice.
  /// </summary>
  /// <param name="invoice"></param>
  /// <returns></returns>
  public async Task DeleteSavedInvoice(Invoice invoice)
  {
    if (invoice == null)
      throw new ArgumentNullException(nameof(invoice));
    await DeleteInvoiceInternalAsync(ResourceInvoicesSaved, invoice.InvoiceNr, _cachedSavedInvoices);
  }

  /// <summary>
  ///   Delete a saved invoice.
  /// </summary>
  /// <param name="invoiceNr"></param>
  /// <returns></returns>
  public async Task DeleteSavedInvoice(string invoiceNr)
  {
    await DeleteInvoiceInternalAsync(ResourceInvoicesSaved, invoiceNr, _cachedSavedInvoices);
  }
}