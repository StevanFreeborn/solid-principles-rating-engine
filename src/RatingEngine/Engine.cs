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
    _logger.Log("Starting rating");
    _logger.Log("Loading policy");

    var policyJson = _source.GetPolicyFromSource();
    var policy = _serializer.DeserializePolicy(policyJson);

    if (policy is null)
    {
      _logger.Log("Failed to load policy.");
      return 0;
    }

    if (policy is UnknownPolicy)
    {
      _logger.Log("Unknown policy type");
    }

    var rating = policy.CalculateRate();

    _logger.Log("Rating completed.");

    return rating;
  }
}