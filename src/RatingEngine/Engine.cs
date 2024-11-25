using System.Text.Json;
using System.Text.Json.Serialization;

namespace RatingEngine;

class Engine
{
  private readonly ConsoleLogger _logger = new();
  private readonly FilePolicySource _source = new();
  private readonly PolicySerializer _serializer = new();

  public Engine()
  {
  }

  public Engine(ConsoleLogger logger, FilePolicySource source, PolicySerializer serializer)
  {
    _logger = logger;
    _source = source;
    _serializer = serializer;
  }

  public decimal Rate()
  {
    _logger.Log("Starting rate.");
    _logger.Log("Loading policy.");

    var policyJson = _source.GetPolicyFromSource();
    var policy = _serializer.DeserializePolicy(policyJson);

    if (policy is null)
    {
      _logger.Log("Failed to load policy.");
      return 0;
    }

    var rating = policy switch
    {
      AutoPolicy auto => RateAutoPolicy(auto),
      LandPolicy land => RateLandPolicy(land),
      LifePolicy life => RateLifePolicy(life),
      _ => RateUnknownPolicy(policy)
    };

    _logger.Log("Rating completed.");

    return rating;
  }

  private decimal RateUnknownPolicy(Policy policy)
  {
    _logger.Log("Unknown policy type.");
    return 0;
  }

  private decimal RateAutoPolicy(AutoPolicy auto)
  {
    _logger.Log("Rating AUTO policy...");
    _logger.Log("Validating policy.");

    if (string.IsNullOrEmpty(auto.Make))
    {
      _logger.Log("Auto policy must specify Make");
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
    _logger.Log("Rating LAND policy...");
    _logger.Log("Validating policy.");

    if (land.BondAmount is 0 || land.Valuation is 0)
    {
      _logger.Log("Land policy must specify Bond Amount and Valuation.");
      return 0;
    }

    if (land.BondAmount < 0.8m * land.Valuation)
    {
      _logger.Log("Insufficient bond amount.");
      return 0;
    }

    return land.BondAmount * 0.05m;
  }

  private decimal RateLifePolicy(LifePolicy life)
  {
    _logger.Log("Rating LIFE policy...");
    _logger.Log("Validating policy.");

    if (life.DateOfBirth == DateTime.MinValue)
    {
      _logger.Log("Life policy must include Date of Birth.");
      return 0;
    }
    if (life.DateOfBirth < DateTime.Today.AddYears(-100))
    {
      _logger.Log("Centenarians are not eligible for coverage.");
      return 0;
    }
    if (life.Amount is 0)
    {
      _logger.Log("Life policy must include an Amount.");
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