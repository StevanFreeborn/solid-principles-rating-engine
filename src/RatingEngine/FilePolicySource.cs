namespace RatingEngine;

class FilePolicySource
{
  public string GetPolicyFromSource()
  {
    var policyPath = Path.Combine(AppContext.BaseDirectory, "policy.json");
    return File.ReadAllText(policyPath);
  }
}