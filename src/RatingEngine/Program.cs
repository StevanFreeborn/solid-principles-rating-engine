using RatingEngine;

Console.WriteLine("Insurance Rating System Starting...");

var engine = new Engine();
var rating = engine.Rate();

if (rating > 0)
{
  Console.WriteLine($"Rating: {rating}");
}
else
{
  Console.WriteLine("No rating produced.");
}