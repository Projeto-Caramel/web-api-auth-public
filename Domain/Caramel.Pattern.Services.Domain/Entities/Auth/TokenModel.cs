using Caramel.Pattern.Services.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Auth
{
    [ExcludeFromCodeCoverage]
    public class TokenModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Token { get; set; }
        public Roles Role { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
