namespace RandomDataFetcher.Domain.Exceptions;

public class ApiCallFailed
    : GeneralException
{
    public ApiCallFailed(string message) : base(message)
    {
    }
}
