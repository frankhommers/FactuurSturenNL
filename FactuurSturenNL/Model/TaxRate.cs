using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))] // This custom converter was placed in a system namespace.
public enum TaxRate
{
  /// <summary>
  ///   No taxes
  /// </summary>
  [EnumMember(Value = "N")] No,

  /// <summary>
  ///   High taxes
  /// </summary>
  [EnumMember(Value = "H")] High,

  /// <summary>
  ///   Low taxes
  /// </summary>
  [EnumMember(Value = "L")] Low,

  /// <summary>
  ///   Zero tax rate
  /// </summary>
  [EnumMember(Value = "Z")] Zero
}