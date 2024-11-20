namespace RatingEngine;

abstract class Policy(string type)
{
  public string Type { get; init; } = type;
}

class LifePolicy : Policy
{
  public string FullName { get; init; } = string.Empty;
  public DateTime DateOfBirth { get; init; }
  public bool IsSmoker { get; init; }
  public decimal Amount { get; init; }

  public LifePolicy() : base(PolicyType.Life) { }
}

class LandPolicy : Policy
{
  public string Address { get; init; } = string.Empty;
  public decimal Size { get; init; }
  public decimal Valuation { get; init; }
  public decimal BondAmount { get; init; }

  public LandPolicy() : base(PolicyType.Land) { }
}

class AutoPolicy : Policy
{
  public string Make { get; init; } = string.Empty;
  public string Model { get; init; } = string.Empty;
  public int Year { get; init; }
  public int Miles { get; init; }
  public decimal Deductible { get; init; }

  public AutoPolicy() : base(PolicyType.Auto) { }
}