using System.Text.Json;

namespace RatingEngine.Tests;

public class RatingEngineRate
{
  private void WritePolicyFile(Policy policy)
  {
    var json = JsonSerializer.Serialize(policy, Json.Options);
    var path = Path.Combine(AppContext.BaseDirectory, "policy.json");
    File.WriteAllText(path, json);
  }

  [Fact]
  public void ReturnsRatingOf10000For200000LandPolicy()
  {
    var policy = new LandPolicy
    {
      BondAmount = 200000,
      Valuation = 200000
    };

    WritePolicyFile(policy);

    var engine = new Engine();
    var result = engine.Rate();

    Assert.Equal(10000, result);
  }

  [Fact]
  public void ReturnsRatingOf0For200000BondOn260000LandPolicy()
  {
    var policy = new LandPolicy
    {
      BondAmount = 200000,
      Valuation = 260000
    };

    WritePolicyFile(policy);

    var engine = new Engine();
    var result = engine.Rate();

    Assert.Equal(0, result);
  }
}
