using System.Text.Json;
using System.Text.Json.Serialization;

namespace RatingEngine;

class Engine
{
  public decimal Rate()
  {
    Console.WriteLine("Starting rate.");
    Console.WriteLine("Loading policy.");

    var policyPath = Path.Combine(AppContext.BaseDirectory, "policy.json");
    var policyJson = File.ReadAllText(policyPath);

    var policy = JsonSerializer.Deserialize<Policy>(policyJson, Json.Options);

    if (policy is null)
    {
      Console.WriteLine("Failed to load policy.");
      return 0;
    }

    var rating = policy switch
    {
      AutoPolicy auto => RateAutoPolicy(auto),
      LandPolicy land => RateLandPolicy(land),
      LifePolicy life => RateLifePolicy(life),
      _ => RateUnknownPolicy(policy)
    };

    Console.WriteLine("Rating completed.");

    return rating;
  }

  private decimal RateUnknownPolicy(Policy policy)
  {
    Console.WriteLine("Unknown policy type.");
    return 0;
  }

  private decimal RateAutoPolicy(AutoPolicy auto)
  {
    Console.WriteLine("Rating AUTO policy...");
    Console.WriteLine("Validating policy.");

    if (string.IsNullOrEmpty(auto.Make))
    {
      Console.WriteLine("Auto policy must specify Make");
      return 0;
    }

    if (auto.Make is "BMW")
    {
      if (auto.Deductible < 500)
      {
        return 1000m;
      }

      return 900m;
    }

    return 0;
  }

  private decimal RateLandPolicy(LandPolicy land)
  {
    Console.WriteLine("Rating LAND policy...");
    Console.WriteLine("Validating policy.");

    if (land.BondAmount is 0 || land.Valuation is 0)
    {
      Console.WriteLine("Land policy must specify Bond Amount and Valuation.");
      return 0;
    }

    if (land.BondAmount < 0.8m * land.Valuation)
    {
      Console.WriteLine("Insufficient bond amount.");
      return 0;
    }

    return land.BondAmount * 0.05m;
  }

  private decimal RateLifePolicy(LifePolicy life)
  {
    Console.WriteLine("Rating LIFE policy...");
    Console.WriteLine("Validating policy.");

    if (life.DateOfBirth == DateTime.MinValue)
    {
      Console.WriteLine("Life policy must include Date of Birth.");
      return 0;
    }
    if (life.DateOfBirth < DateTime.Today.AddYears(-100))
    {
      Console.WriteLine("Centenarians are not eligible for coverage.");
      return 0;
    }
    if (life.Amount is 0)
    {
      Console.WriteLine("Life policy must include an Amount.");
      return 0;
    }

    int age = DateTime.Today.Year - life.DateOfBirth.Year;

    bool isBirthdayAlreadyPassed = life.DateOfBirth.Month == DateTime.Today.Month &&
      DateTime.Today.Day < life.DateOfBirth.Day ||
      DateTime.Today.Month < life.DateOfBirth.Month;

    if (isBirthdayAlreadyPassed)
    {
      age--;
    }

    var baseRate = life.Amount * age / 200;

    if (life.IsSmoker)
    {
      return baseRate * 2;
    }

    return baseRate;
  }
}