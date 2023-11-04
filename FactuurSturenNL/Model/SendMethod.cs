using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum SendMethod
{
  [EnumMember(Value = "none")] None,

  /// <summary>
  ///   Print the invoices yourself. We'll send you the invoice number so you can execute a command to retrieve the PDF if
  ///   you need so
  /// </summary>
  [EnumMember(Value = "mail")] Mail,

  /// <summary>
  ///   Send invoices through e-mail. It will be sent immediately
  /// </summary>
  [EnumMember(Value = "email")] Email,

  /// <summary>
  ///   Send invoice through the printcenter.
  /// </summary>
  [EnumMember(Value = "printcenter")] Printcenter
}