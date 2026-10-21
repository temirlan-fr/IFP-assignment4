using DeliveryQuotePipeline.App;
using System.Linq;

string[] inputLines =
{
    "R-001,Aruzhan,2,3,false",
    "R-002,Timur,50,20,true",
    "R-003,Ali,-1,2,false",
    "R-004,Dana,12,1,true",
    "R-005,Mira,0,0.5,false"
};

List<DeliveryRequest> validRequests = new();

foreach (string inputLine in inputLines)
{
    bool isValid = DeliveryRequestParser.TryParse(
        inputLine,
        out DeliveryRequest? request,
        out string error);

    if (isValid && request is not null)
    {
        validRequests.Add(request);
    }
    else
    {
        Console.WriteLine($"Rejected: {error}");
    }
}

IReadOnlyList<DeliveryRequest> readOnlyRequests =
    Array.AsReadOnly(validRequests.ToArray());

IReadOnlyList<DeliveryRequest> selectedRequests =
    QuotePipeline.SelectEligible(readOnlyRequests);

Func<DeliveryRequest, decimal> standardPolicy =
    PricingPolicies.Standard;

Func<DeliveryRequest, decimal> expressPolicy =
    PricingPolicies.Express;

IReadOnlyList<DeliveryQuote> standardQuotes =
    QuotePipeline.CalculateQuotes(
        selectedRequests,
        "Standard",
        standardPolicy);

IReadOnlyList<DeliveryQuote> expressQuotes =
    QuotePipeline.CalculateQuotes(
        selectedRequests,
        "Express",
        expressPolicy);

DeliveryQuoteSummary standardSummary =
    QuotePipeline.CreateSummary(standardQuotes);

DeliveryQuoteSummary expressSummary =
    QuotePipeline.CreateSummary(expressQuotes);

Console.WriteLine();
Console.WriteLine("Standard policy:");

foreach (DeliveryQuote quote in standardSummary.Quotes)
{
    Console.WriteLine(
        $"{quote.RequestId} {quote.CustomerName}: {quote.Price}");
}

Console.WriteLine();
Console.WriteLine("Express policy:");

foreach (DeliveryQuote quote in expressSummary.Quotes)
{
    Console.WriteLine(
        $"{quote.RequestId} {quote.CustomerName}: {quote.Price}");
}