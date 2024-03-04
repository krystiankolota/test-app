using System.Net;
namespace RandomDataFetcher.Domain.Exceptions;

public class BlobOrBlobContainerNotFound : GeneralException
{
    public BlobOrBlobContainerNotFound(string message)
        : base(message)
    {
    }

    public override int StatusCode => (int) HttpStatusCode.NotFound;
}
