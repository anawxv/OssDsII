using OsDsII.Exceptions.Constraints;
using System.Net;

namespace OsDsII.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) :
        base
            (
                ErrorCode.BAD_REQUEST,
                message,
                HttpStatusCode.NotFound,
                StatusCodes.Status404NotFound,
                null,
                DateTimeOffset.UtcNow,
                null
            )
        { }
    }
}