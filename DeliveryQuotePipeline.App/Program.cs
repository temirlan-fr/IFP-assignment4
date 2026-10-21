using DeliveryQuotePipeline.App;

string[] inputLines =
{
    "R-001,Aruzhan,2,3,false",
    "R-002,Timur,50,20,true",
    "R-003,Ali,-1,2,false",
    "R-004,Dana,12,1,true",
    "R-005,Mira,0,0.5,false"
};

foreach (string inputLine in inputLines)
{
    bool isValid = DeliveryRequestParser.TryParse(
        inputLine,
        out DeliveryRequest? request,
        out string error);

    if (isValid && request is not null)
    {
        Console.WriteLine(
            $"{request.RequestId} accepted for {request.CustomerName}");
    }
    else
    {
        Console.WriteLine($"{inputLine} rejected: {error}");
    }
}