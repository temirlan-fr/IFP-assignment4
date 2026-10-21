using DeliveryQuotePipeline.App;

namespace DeliveryQuotePipeline.Tests;

public class PipelineTests
{
    [Fact]
    public void NormalWorkflowProducesExpectedResult()
    {
        DeliveryRequest[] requests =
        {
            new("R-001", "Aruzhan", 2, 3, false)
        };

        IReadOnlyList<DeliveryRequest> selected =
            QuotePipeline.SelectEligible(requests);

        IReadOnlyList<DeliveryQuote> quotes =
            QuotePipeline.CalculateQuotes(
                selected,
                "Standard",
                PricingPolicies.Standard);

        DeliveryQuoteSummary summary =
            QuotePipeline.CreateSummary(quotes);

        Assert.Single(summary.Quotes);
        Assert.Equal(700m, summary.Quotes[0].Price);
    }

    [Fact]
    public void EmptyDatasetProducesEmptyResult()
    {
        DeliveryRequest[] requests = Array.Empty<DeliveryRequest>();

        IReadOnlyList<DeliveryRequest> selected =
            QuotePipeline.SelectEligible(requests);

        IReadOnlyList<DeliveryQuote> quotes =
            QuotePipeline.CalculateQuotes(
                selected,
                "Standard",
                PricingPolicies.Standard);

        DeliveryQuoteSummary summary =
            QuotePipeline.CreateSummary(quotes);

        Assert.Empty(summary.Quotes);
    }

    [Fact]
    public void BoundaryDistanceIsAccepted()
    {
        DeliveryRequest request =
            new("R-002", "Timur", 50, 20, true);

        IReadOnlyList<DeliveryQuote> quotes =
            QuotePipeline.CalculateQuotes(
                new[] { request },
                "Standard",
                PricingPolicies.Standard);

        Assert.Equal(5700m, quotes[0].Price);
    }

    [Fact]
    public void InvalidAndMissingInputAreRejected()
    {
        bool invalidResult = DeliveryRequestParser.TryParse(
            "R-003,Ali,-1,2,false",
            out DeliveryRequest? invalidRequest,
            out string invalidError);

        bool missingResult = DeliveryRequestParser.TryParse(
            null,
            out DeliveryRequest? missingRequest,
            out string missingError);

        Assert.False(invalidResult);
        Assert.Null(invalidRequest);
        Assert.NotEmpty(invalidError);

        Assert.False(missingResult);
        Assert.Null(missingRequest);
        Assert.Equal("Input is missing.", missingError);
    }

    [Fact]
    public void StandardAndExpressPoliciesProduceDifferentResults()
    {
        DeliveryRequest request =
            new("R-001", "Aruzhan", 2, 3, false);

        IReadOnlyList<DeliveryQuote> standardQuotes =
            QuotePipeline.CalculateQuotes(
                new[] { request },
                "Standard",
                PricingPolicies.Standard);

        IReadOnlyList<DeliveryQuote> expressQuotes =
            QuotePipeline.CalculateQuotes(
                new[] { request },
                "Express",
                PricingPolicies.Express);

        Assert.Equal(700m, standardQuotes[0].Price);
        Assert.Equal(1100m, expressQuotes[0].Price);
        Assert.NotEqual(
            standardQuotes[0].Price,
            expressQuotes[0].Price);
    }

    [Fact]
    public void PipelineDoesNotChangeCallerOwnedCollection()
    {
        List<DeliveryRequest> requests =
        [
            new("R-001", "Aruzhan", 2, 3, false),
            new("R-002", "Timur", 50, 20, true)
        ];

        DeliveryRequest[] originalValues = requests.ToArray();

        QuotePipeline.SelectEligible(requests);

        Assert.Equal(originalValues, requests);
        Assert.Equal(2, requests.Count);
    }
}