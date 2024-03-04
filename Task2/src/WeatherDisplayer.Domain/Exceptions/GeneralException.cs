namespace WeatherDisplayer.Domain.Exceptions;

public class GeneralException : Exception
{
    public GeneralException(string message)
        : base(message)
    {
    }
    public virtual int StatusCode => 500;
}
