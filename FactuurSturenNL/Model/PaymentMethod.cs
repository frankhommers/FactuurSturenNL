using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum PaymentMethod
{
  [EnumMember(Value = "none")] None,
  [EnumMember(Value = "bank")] Bank,
  [EnumMember(Value = "autocollect")] AutoCollect
}