using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Responses
{
    [ExcludeFromCodeCoverage]
    public class ExceptionResponse
    {
        public StatusProcess Status { get; set; }
        public string Description { get; set; }
        public string[] ErrorDetails { get; set; }

        public ExceptionResponse(StatusProcess status, string[] details)
        {
            Status = status;
            Description = status.GetDescription();
            ErrorDetails = details;
        }

        public ExceptionResponse(StatusProcess status, string details)
        {
            Status = status;
            Description = status.GetDescription();
            ErrorDetails = [details];
        }
    }
}
