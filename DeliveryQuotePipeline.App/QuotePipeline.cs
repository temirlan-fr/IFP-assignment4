namespace DeliveryQuotePipeline.App;

public static class QuotePipeline
{
    public static IReadOnlyList<DeliveryRequest> SelectEligible(
        IReadOnlyList<DeliveryRequest> requests)
    {
        DeliveryRequest[] selected = requests
            .Where(request =>
                request.DistanceKm >= 0 &&
                request.DistanceKm <= 50 &&
                request.WeightKg > 0 &&
                request.WeightKg <= 20)
            .ToArray();

        return Array.AsReadOnly(selected);
    }

    public static IReadOnlyList<DeliveryQuote> CalculateQuotes(
        IReadOnlyList<DeliveryRequest> requests,
        string policyName,
        Func<DeliveryRequest, decimal> pricePolicy)
    {
        DeliveryQuote[] quotes = requests
            .Select(request =>
            {
                decimal price = pricePolicy(request);

                return new DeliveryQuote(
                    request.RequestId,
                    request.CustomerName,
                    policyName,
                    price);
            })
            .ToArray();

        return Array.AsReadOnly(quotes);
    }

    public static DeliveryQuoteSummary CreateSummary(
        IReadOnlyList<DeliveryQuote> quotes)
    {
        DeliveryQuote[] orderedQuotes = quotes
            .OrderBy(quote => quote.Price)
            .ToArray();

        return new DeliveryQuoteSummary(
            Array.AsReadOnly(orderedQuotes));
    }
}