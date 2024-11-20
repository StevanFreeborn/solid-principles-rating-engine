using System.Text.Json;
using System.Text.Json.Serialization;

namespace RatingEngine;

static class Json
{
  public static JsonSerializerOptions Options = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    PropertyNameCaseInsensitive = true,
    Converters = { new PolicyConverter() }
  };
}