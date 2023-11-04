using System.Text.Json.Serialization;
using FactuurSturenNL.Converters;

namespace FactuurSturenNL.Model;

public class Client
{
  /// <summary>
  ///   Date and time when record was updated
  /// </summary>
  [JsonPropertyName("timestamp")]
  public DateTime? TimeStamp { get; set; }

  [JsonConverter(typeof(IntAsStringConverter))]
  [JsonPropertyName("clientnr")]
  public int ClientNr { get; set; }

  [JsonPropertyName("contact")] public string Contact { get; set; }

  /// <summary>
  ///   Show the contact name on the invoice
  /// </summary>
  [JsonConverter(typeof(BoolAsStringFalseAndTrueConverter))]
  [JsonPropertyName("showcontact")]
  public bool ShowContact { get; set; }

  [JsonPropertyName("company")] public string Company { get; set; }
  [JsonPropertyName("address")] public string Address { get; set; }
  [JsonPropertyName("zipcode")] public string Zipcode { get; set; }
  [JsonPropertyName("city")] public string City { get; set; }
  [JsonPropertyName("country")] public string Country { get; set; }
  [JsonPropertyName("phone")] public string Phone { get; set; }
  [JsonPropertyName("mobile")] public string Mobile { get; set; }

  /// <summary>
  ///   Invoice is sent to this e-mail address, if the sendmethod is e-mail
  /// </summary>
  [JsonPropertyName("email")]
  public string Email { get; set; }

  /// <summary>
  ///   The IBAN number of the client
  /// </summary>
  [JsonPropertyName("bankcode")]
  public string BankCode { get; set; }

  [JsonPropertyName("biccode")] public string BicCode { get; set; }
  [JsonPropertyName("taxnumber")] public string TaxNumber { get; set; }

  /// <summary>
  ///   If the taxes on the invoice is shifted to the receiver
  /// </summary>
  [JsonPropertyName("taxshifted")]
  public bool TaxShifted { get; set; }

  /// <summary>
  ///   When last invoice to this client was sent
  /// </summary>
  [JsonPropertyName("lastinvoice")]
  public DateTime? LastInvoice { get; set; }

  /// <summary>
  ///   How to send the invoice to the receiver.
  /// </summary>
  [JsonPropertyName("sendmethod")]
  public SendMethod SendMethod { get; set; }

  /// <summary>
  ///   How the invoice is going to be paid
  /// </summary>
  [JsonPropertyName("paymentmethod")]
  public PaymentMethod PaymentMethod { get; set; }

  /// <summary>
  ///   The term of payment in days. Defines when the invoice has to be paid by the recipient
  /// </summary>


  [JsonConverter(typeof(IntAsStringConverter))]
  [JsonPropertyName("top")]
  public int TermOfPaymentDays { get; set; }

  /// <summary>
  ///   Standard discount percentage for this client. Every invoice defined for this client will automatically get this
  ///   discount percentage
  /// </summary>
  [JsonConverter(typeof(IntAsStringConverter))]
  [JsonPropertyName("stddiscount")]
  public int StandardDiscountPercentage { get; set; }

  /// <summary>
  ///   The first line used in the e-mail to address the recipient. Example: "Dear sir/madam,"
  /// </summary>
  [JsonPropertyName("mailintro")]
  public string MailIntro { get; set; }

  /// <summary>
  ///   Three lines that will be printed on the invoice. Can be used for references to other documents or something else
  /// </summary>
  [JsonPropertyName("reference")]
  public Reference Reference { get; set; }

  /// <summary>
  ///   Notes saved for this client
  /// </summary>
  [JsonPropertyName("notes")]
  public string Notes { get; set; }

  /// <summary>
  ///   Print the field 'notes' on every invoice for the client
  /// </summary>
  [JsonPropertyName("notesoninvoice")]
  public bool NotesOnInvoice { get; set; }

  /// <summary>
  ///   Non-active clients are hidden in the web application.
  /// </summary>
  [JsonConverter(typeof(BoolAsStringFalseAndTrueConverter))]
  [JsonPropertyName("active")]
  public bool Active { get; set; }

  /// <summary>
  ///   In what language the invoice will be generated for this client.
  /// </summary>
  [JsonPropertyName("defaultdoclang")]
  public Language DefaultDocLang { get; set; }

  /// <summary>
  ///   ID of used e-mail text
  /// </summary>
  [JsonPropertyName("defaultemail")]
  public int DefaultEmail { get; set; }

  /// <summary>
  ///   Used currency in invoice. Like 'EUR', 'USD', etc.
  /// </summary>
  [JsonPropertyName("currency")]
  public string Currency { get; set; }

  /// <summary>
  ///   The mandate identification
  /// </summary>
  [JsonPropertyName("mandateid")]
  public string MandateId { get; set; }

  /// <summary>
  ///   The date of the signature
  /// </summary>
  [JsonPropertyName("mandatedate")]
  public string MandateDate { get; set; }

  /// <summary>
  ///   The collection type
  /// </summary>
  [JsonPropertyName("collecttype")]
  public CollectType CollectType { get; set; }

  /// <summary>
  ///   Will show if the products on the invoice for this client will be handled as excluding or including tax
  /// </summary>
  [JsonPropertyName("taxtype")]
  public TaxType TaxType { get; set; }

  [JsonPropertyName("defaultcategory")] public string DefaultCategory { get; set; }
}