namespace DeliveryQuotePipeline.App;

public sealed record DeliveryRequest(
    string RequestId,
    string CustomerName,
    decimal DistanceKm,
    decimal WeightKg,
    bool IsFragile);