namespace RatingEngine;

abstract class Policy(string type)
{
  public string Type { get; init; } = type;

  public virtual decimal CalculateRate()
  {
    return 0;
  }
}

class LifePolicy : Policy
{
  public string FullName { get; init; } = string.Empty;
  public DateTime DateOfBirth { get; init; }
  public bool IsSmoker { get; init; }
  public decimal Amount { get; init; }

  public LifePolicy() : base(PolicyType.Life) { }

  public override decimal CalculateRate()
  {
    if (DateOfBirth == DateTime.MinValue)
    {
      return 0;
    }
    if (DateOfBirth < DateTime.Today.AddYears(-100))
    {
      return 0;
    }
    if (Amount is 0)
    {
      return 0;
    }

    int age = DateTime.Today.Year - DateOfBirth.Year;

    bool isBirthdayAlreadyPassed = DateOfBirth.Month == DateTime.Today.Month &&
      DateTime.Today.Day < DateOfBirth.Day ||
      DateTime.Today.Month < DateOfBirth.Month;

    if (isBirthdayAlreadyPassed)
    {
      age--;
    }

    var baseRate = Amount * age / 200;

    if (IsSmoker)
    {
      return baseRate * 2;
    }

    return baseRate;
  }
}

class LandPolicy : Policy
{
  public string Address { get; init; } = string.Empty;
  public decimal Size { get; init; }
  public decimal Valuation { get; init; }
  public decimal BondAmount { get; init; }

  public LandPolicy() : base(PolicyType.Land) { }

  public override decimal CalculateRate()
  {
    if (BondAmount is 0 || Valuation is 0)
    {
      return 0;
    }

    if (BondAmount < 0.8m * Valuation)
    {
      return 0;
    }

    return BondAmount * 0.05m;
  }
}

class AutoPolicy : Policy
{
  public string Make { get; init; } = string.Empty;
  public string Model { get; init; } = string.Empty;
  public int Year { get; init; }
  public int Miles { get; init; }
  public decimal Deductible { get; init; }

  public AutoPolicy() : base(PolicyType.Auto) { }

  public override decimal CalculateRate()
  {
    if (string.IsNullOrEmpty(Make))
    {
      return 0;
    }

    if (Make is "BMW")
    {
      if (Deductible < 500)
      {
        return 1000m;
      }

      return 900m;
    }

    return 0;
  }
}

class UnknownPolicy : Policy
{
  public UnknownPolicy() : base(PolicyType.Unknown) { }
}