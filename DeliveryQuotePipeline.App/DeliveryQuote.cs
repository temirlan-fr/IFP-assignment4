namespace DeliveryQuotePipeline.App;

public sealed record DeliveryQuote(
    string RequestId,
    string CustomerName,
    string PolicyName,
    decimal Price);

public sealed record DeliveryQuoteSummary(
    IReadOnlyList<DeliveryQuote> Quotes);