namespace WeatherDisplayer.Domain.Exceptions;

public class InvalidApiResponse : Exception
{
    public InvalidApiResponse(string message)
        : base(message)
    {
    }
}
