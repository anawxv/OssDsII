using System.Net;
using OsDsII.Exceptions.Constraints;
namespace OsDsII.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string resource) :
            base
            (
                ErrorCode.NOT_FOUND,
                $"{resource} not found",
                HttpStatusCode.NotFound,
                StatusCodes.Status404NotFound,
                null,
                DateTimeOffset.UtcNow,
                null
            )
        { }

        public NotFoundException(string message, string uriPath, Exception ex) :
            base
            (
                ErrorCode.NOT_FOUND,
                message,
                HttpStatusCode.NotFound,
                StatusCodes.Status404NotFound,
                uriPath,
                DateTimeOffset.UtcNow,
                ex
            )
        { }
    }
}