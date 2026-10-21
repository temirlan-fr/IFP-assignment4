namespace DeliveryQuotePipeline.App;

public static class PricingPolicies
{
    public static decimal Standard(DeliveryRequest request)
    {
        decimal price = 500 + request.DistanceKm * 100;

        if (request.IsFragile)
        {
            price += 200;
        }

        return price;
    }

    public static decimal Express(DeliveryRequest request)
    {
        decimal price = 800 + request.DistanceKm * 150;

        if (request.IsFragile)
        {
            price += 200;
        }

        return price;
    }
}