using Caramel.Pattern.Services.Domain.Enums;
using System.Net;

namespace Caramel.Pattern.Services.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() { }

        public BusinessException(string[] details, StatusProcess status, HttpStatusCode statusCode)
        {
            Status = status;
            StatusCode = statusCode;
            ErrorDetails = details;
        }

        public BusinessException(string details, StatusProcess status, HttpStatusCode statusCode)
        {
            Status = status;
            StatusCode = statusCode;
            ErrorDetails = [details];
        }

        public StatusProcess Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string[] ErrorDetails { get; set; }

        public static void ThrowIfNull(object argument, string argumentName)
        {
            if (argument == null)
                throw new BusinessException(
                    $"O parâmetro {argumentName} não pode ser nulo.",
                    StatusProcess.InvalidRequest,
                    HttpStatusCode.UnprocessableEntity
                );
        }
    }
}

