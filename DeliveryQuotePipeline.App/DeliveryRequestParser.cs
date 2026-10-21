using System.Globalization;

namespace DeliveryQuotePipeline.App;

public static class DeliveryRequestParser
{
    public static bool TryParse(
        string? rawLine,
        out DeliveryRequest? request,
        out string error)
    {
        request = null;
        error = string.Empty;

        if (rawLine is null)
        {
            error = "Input is missing.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(rawLine))
        {
            error = "Input is empty.";
            return false;
        }

        string[] fields = rawLine.Split(',');

        if (fields.Length != 5)
        {
            error = "Input must contain exactly five fields.";
            return false;
        }

        string requestId = fields[0].Trim();
        string customerName = fields[1].Trim();

        if (string.IsNullOrWhiteSpace(requestId))
        {
            error = "RequestId is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(customerName))
        {
            error = "CustomerName is required.";
            return false;
        }

        if (!decimal.TryParse(
                fields[2].Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal distanceKm))
        {
            error = "DistanceKm must be a number.";
            return false;
        }

        if (distanceKm < 0 || distanceKm > 50)
        {
            error = "DistanceKm must be between 0 and 50.";
            return false;
        }

        if (!decimal.TryParse(
                fields[3].Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal weightKg))
        {
            error = "WeightKg must be a number.";
            return false;
        }

        if (weightKg <= 0 || weightKg > 20)
        {
            error = "WeightKg must be greater than 0 and no more than 20.";
            return false;
        }

        if (!bool.TryParse(fields[4].Trim(), out bool isFragile))
        {
            error = "IsFragile must be true or false.";
            return false;
        }

        request = new DeliveryRequest(
            requestId,
            customerName,
            distanceKm,
            weightKg,
            isFragile);

        return true;
    }
}