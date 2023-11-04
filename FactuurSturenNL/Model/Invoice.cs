using System.Text.Json.Serialization;
using FactuurSturenNL.Converters;
using FactuurSturenNL.Exceptions;

namespace FactuurSturenNL.Model;

public class Invoice
{
  public Invoice()
  {
  }


  /// <summary>
  ///   Initializes a new Invoice instance. Can be used to create an invoice.
  /// </summary>
  /// <param name="client"></param>
  /// <param name="action"></param>
  /// <param name="saveName">Required when Action is Save or Repeat</param>
  public Invoice(Client client, InvoiceAction action, string? saveName = null) : this()
  {
    Client = client;
    Action = action;
    SaveName = saveName;
  }

  /// <summary>
  ///   Initializes a new Invoice instance. Can be used to create an invoice.
  /// </summary>
  /// <param name="client"></param>
  /// <param name="action"></param>
  /// <param name="saveName">Required when Action is Save or Repeat</param>
  public Invoice(Client client, InvoiceAction action, SendMethod sendMethod) : this(client, action)
  {
    SendMethod = sendMethod;
  }

  [JsonPropertyName("id")] public string Id { get; set; }

  [JsonPropertyName("invoicenr_full")] public string InvoiceNrFull { get; set; }

  /// <summary>
  ///   Invoice number including layout. When posting an invoice, do not specify or define this attribute.
  ///   If you really want to define the invoice number, please include the invoice number as an integer
  ///   number without any prefix layout. So to create INVOICE00023, please send us 23 as integer.
  /// </summary>
  [JsonPropertyName("invoicenr")]
  public string InvoiceNr { get; set; }

  /// <summary>
  ///   Contains reference lines on the invoice. 'line1', 'line2', 'line3'. All are strings
  /// </summary>
  [JsonPropertyName("reference")]
  public Reference? Reference { get; set; }

  /// <summary>
  ///   All invoice lines on the invoice
  /// </summary>
  [JsonPropertyName("lines")]
  public Dictionary<string, InvoiceLine> Lines { get; set; } = new();

  /// <summary>
  ///   The ID of the used profile. Default is default profile
  /// </summary>
  [JsonConverter(typeof(IntAsStringConverter))]
  [JsonPropertyName("profile")]
  public int? Profile { get; set; }

  /// <summary>
  ///   The type of discount. 'amount' or 'percentage'
  /// </summary>
  [JsonPropertyName("discounttype")]
  public string DiscountType { get; set; }

  /// <summary>
  ///   If 'DiscountType' is amount, then this is the amount of discount set on the invoice.
  ///   If 'DiscountType' is set to 'percentage', this is the discount percentage set.
  /// </summary>
  [JsonConverter(typeof(NullableFloatAsStringConverter))]
  [JsonPropertyName("discount")]
  public float? Discount { get; set; }

  /// <summary>
  ///   The payment condition set on the invoice. Default is the payment condition set in the application.
  /// </summary>
  [JsonPropertyName("paymentcondition")]
  public string PaymentCondition { get; set; }

  /// <summary>
  ///   Term of payment in days.Default is the payment period set with the client.
  /// </summary>
  [JsonConverter(typeof(IntAsStringConverter))]
  [JsonPropertyName("paymentperiod")]
  public int PaymentPeriod { get; set; }

  /// <summary>
  ///   The date the invoice was saved
  /// </summary>
  [JsonPropertyName("datesaved")]
  public DateTime? DateSaved { get; set; }

  /// <summary>
  ///   If invoice is an automatic collection
  /// </summary>
  [JsonConverter(typeof(BoolAsString0And1Converter))]
  [JsonPropertyName("collection")]
  public bool Collection { get; set; }

  /// <summary>
  ///   Total of all taxes on this invoice
  /// </summary>
  [JsonConverter(typeof(FloatAsStringConverter))]
  [JsonPropertyName("tax")]
  public float Tax { get; set; }

  /// <summary>
  ///   Invoice total including taxes
  /// </summary>
  [JsonConverter(typeof(FloatAsStringConverter))]
  [JsonPropertyName("totalintax")]
  public float TotalIntax { get; set; }

  /// <summary>
  ///   Client number
  /// </summary>
  [JsonConverter(typeof(IntAsStringConverter))]
  [JsonPropertyName("clientnr")]
  public int ClientNr { get; set; }

  [JsonIgnore]
  public Client Client
  {
    set => ClientNr = value?.ClientNr ?? 0;
  }

  [JsonPropertyName("company")] public string Company { get; set; }
  [JsonPropertyName("contact")] public string Contact { get; set; }
  [JsonPropertyName("address")] public string Address { get; set; }
  [JsonPropertyName("zipcode")] public string Zipcode { get; set; }
  [JsonPropertyName("city")] public string City { get; set; }

  /// <summary>
  ///   Country id. You can get a list of country id's with the function api/v1/countrylist.
  ///   When creating or updating a client, you can supply a country id or a country name.
  ///   We'll then try to find the id of the country you supplied
  /// </summary>
  [JsonPropertyName("country")]
  public string Country { get; set; }

  [JsonPropertyName("phone")] public string Phone { get; set; }
  [JsonPropertyName("mobile")] public string Mobile { get; set; }
  [JsonPropertyName("email")] public string Email { get; set; }
  [JsonPropertyName("taxnumber")] public string TaxNumber { get; set; }

  /// <summary>
  ///   A note that is saved with the invoice
  /// </summary>
  [JsonPropertyName("invoicenote")]
  public string InvoiceNote { get; set; }

  /// <summary>
  ///   The date the invoice is sent
  /// </summary>
  [JsonPropertyName("sent")]
  public DateTime? Sent { get; set; }

  /// <summary>
  ///   The date when the invoice is marked as uncollectible
  /// </summary>
  [JsonConverter(typeof(NullableDateOnlyAsStringConverter))]
  [JsonPropertyName("uncollectible")]
  public DateOnly? Uncollectible { get; set; }

  /// <summary>
  ///   The date when the last reminder was sent
  /// </summary>
  [JsonConverter(typeof(NullableDateOnlyAsStringConverter))]
  [JsonPropertyName("lastreminder")]
  public DateOnly? LastReminder { get; set; }

  /// <summary>
  ///   The amount that is still open on the invoice. If this amount is 0, then the invoice is paid.If it is negative,
  ///   there is paid more than the invoice amount.
  /// </summary>
  [JsonConverter(typeof(FloatAsStringConverter))]
  [JsonPropertyName("open")]
  public float AmountOpen { get; set; }

  /// <summary>
  ///   The date of the last received payment
  /// </summary>
  [JsonConverter(typeof(NullableDateOnlyAsStringConverter))]
  [JsonPropertyName("paiddate")]
  public DateOnly? PaidDate { get; set; }

  /// <summary>
  ///   All taxes defined in the invoice
  /// </summary>
  [JsonPropertyName("taxes")]
  public Dictionary<string, TaxItem> Taxes { get; set; } = new();

  /// <summary>
  ///   The complete URL where the invoice can be paid
  /// </summary>
  [JsonPropertyName("payment_url")]
  public string PaymentUrl { get; set; }

  /// <summary>
  ///   The duedate of the invoice. This is the sent-date + the payment period in days
  /// </summary>
  [JsonConverter(typeof(NullableDateOnlyAsStringConverter))]
  [JsonPropertyName("duedate")]
  public DateOnly? DueDate { get; set; }

  [JsonPropertyName("history")] public Dictionary<string, HistoryItem> History { get; set; } = new();

  public virtual void AddLine(InvoiceLine line)
  {
    Lines.Add($"line{(Lines.Count + 1)}", line);
  }

  public virtual void AddLines(IEnumerable<InvoiceLine> lines)
  {
    foreach (InvoiceLine line in lines)
      AddLine(line);
  }

  public virtual void AddReferenceLine(string referenceText)
  {
    Reference ??= new Reference();
    
    if (string.IsNullOrWhiteSpace(Reference.Line1))
      Reference.Line1 = referenceText;
    else if (string.IsNullOrWhiteSpace(Reference.Line2))
      Reference.Line2 = referenceText;
    else if (string.IsNullOrWhiteSpace(Reference.Line3))
      Reference.Line3 = referenceText;
    else
      throw new InvalidOperationException("All reference lines are filled. Cannot add a new line.");
  }

  #region Needed for new invoice

  /// <summary>
  ///   Define the action what to do when this is a new invoice request
  /// </summary>
  [JsonPropertyName("action")]
  public InvoiceAction Action { get; set; }

  /// <summary>
  ///   How to send the invoice to the receiver. Required when you use the action 'send'
  /// </summary>
  [JsonPropertyName("sendmethod")]
  public SendMethod SendMethod { get; set; }

  /// <summary>
  ///   When the action is 'save' or 'repeat' you must supply a savename.We'll save the invoice under that name.
  /// </summary>
  [JsonPropertyName("savename")]
  public string? SaveName { get; set; }

  /// <summary>
  ///   If a savename already exists, it will not be overwritten unless this attribute is set to 'true'. Default is 'false'.
  /// </summary>
  [JsonPropertyName("overwrite_if_exist")]
  public bool OverwriteIfExist { get; set; }

  /// <summary>
  ///   When this option is set to 'true' we will convert all the given prices on the invoices to euro,
  ///   based on the currency set in the selected client and the invoice date (to retrieve the current exchange rate)
  /// </summary>
  [JsonPropertyName("convert_prices_to_euro")]
  public bool ConvertPricesToEuro { get; set; }

  #region Needed when Action is Repeat

  /// <summary>
  ///   Date when the first invoice must be sent. Please use YYYY-MM-DD
  /// </summary>
  [JsonPropertyName("initialdate")]
  public DateTime? InitialDate { get; set; }

  /// <summary>
  ///   The next date when the next invoice is going to be sent.
  /// </summary>
  [JsonPropertyName("nextsenddate")]
  public DateTime? NextSendDate { get; set; }


  /// <summary>
  ///   Date when the last invoice must be sent. After this date the recurring invoice entry is deleted.
  /// </summary>
  [JsonPropertyName("finalsenddate")]
  public DateTime? FinalSendDate { get; set; }

  /// <summary>
  ///   The frequency when the invoice must be sent. Based on the initialdate.
  /// </summary>
  [JsonPropertyName("frequency")]
  public Frequency Frequency { get; set; }

  /// <summary>
  ///   Set if the recurring invoice is automatically sent by our system
  /// </summary>
  [JsonPropertyName("repeattype")]
  public RepeatType RepeatType { get; set; }

  #endregion

  #endregion
}