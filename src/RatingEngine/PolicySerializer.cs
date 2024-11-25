using System.Text.Json;

namespace RatingEngine;

class PolicySerializer
{
  public Policy? DeserializePolicy(string policyJson)
  {
    return JsonSerializer.Deserialize<Policy>(policyJson, Json.Options);
  }
}