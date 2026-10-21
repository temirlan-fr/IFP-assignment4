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

IReadOnlyList<DeliveryQuote> quotes =
    QuotePipeline.CalculateQuotes(selectedRequests);

DeliveryQuoteSummary summary =
    QuotePipeline.CreateSummary(quotes);

Console.WriteLine();
Console.WriteLine("Delivery quotes:");

foreach (DeliveryQuote quote in summary.Quotes)
{
    Console.WriteLine(
        $"{quote.RequestId} {quote.CustomerName}: {quote.Price}");
}