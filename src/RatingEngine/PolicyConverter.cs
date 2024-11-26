using System.Text.Json;
using System.Text.Json.Serialization;

namespace RatingEngine;

class PolicyConverter : JsonConverter<Policy>
{
  public override Policy? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    using var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    var type = root.GetProperty("type").GetString();
    return type switch
    {
      PolicyType.Life => JsonSerializer.Deserialize<LifePolicy>(root.GetRawText(), options),
      PolicyType.Land => JsonSerializer.Deserialize<LandPolicy>(root.GetRawText(), options),
      PolicyType.Auto => JsonSerializer.Deserialize<AutoPolicy>(root.GetRawText(), options),
      _ => new UnknownPolicy(),
    };
  }

  public override void Write(Utf8JsonWriter writer, Policy value, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, value, value.GetType(), options);
  }
}