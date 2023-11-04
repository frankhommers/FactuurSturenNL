using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))] // This custom converter was placed in a system namespace.
public enum InvoiceAction
{
  [EnumMember(Value = "none")] None,

  /// <summary>
  ///   Send the invoice
  /// </summary>
  [EnumMember(Value = "send")] Send,

  /// <summary>
  ///   Save the invoice as a draft
  /// </summary>
  [EnumMember(Value = "save")] Save,

  /// <summary>
  ///   Plan a recurring invoice
  /// </summary>
  [EnumMember(Value = "repeat")] Repeat
}