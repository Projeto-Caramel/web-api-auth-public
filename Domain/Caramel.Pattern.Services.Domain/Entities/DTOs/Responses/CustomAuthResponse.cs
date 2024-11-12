using Caramel.Pattern.Services.Domain.Entities.Auth;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Responses
{
    [ExcludeFromCodeCoverage]
    public class CustomAuthResponse<T>
    {
        public CustomAuthResponse(StatusProcess status, TokenModel token)
        {
            Token = token;
            Status = status;
            Description = status.GetDescription();
        }

        public TokenModel Token { get; set; }
        public StatusProcess Status { get; set; }
        public string Description { get; set; }
    }
}
