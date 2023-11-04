using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum Frequency
{
  [EnumMember(Value = "none")] None,
  [EnumMember(Value = "weekly")] Weekly,
  [EnumMember(Value = "monthly")] Monthly,
  [EnumMember(Value = "quarterly")] Quarterly,
  [EnumMember(Value = "halfyearly")] HalfYearly,
  [EnumMember(Value = "yearly")] Yearly,
  [EnumMember(Value = "biweekly")] BiWeekly,
  [EnumMember(Value = "bimonthly")] BiMonthly,
  [EnumMember(Value = "fourweekly")] FourWeekly
}