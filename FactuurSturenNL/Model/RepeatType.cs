using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))] // This custom converter was placed in a system namespace.
public enum RepeatType
{
  None,

  /// <summary>
  ///   Send automatically when due
  /// </summary>
  Auto,

  /// <summary>
  ///   Do not send automatically
  /// </summary>
  Manual
}