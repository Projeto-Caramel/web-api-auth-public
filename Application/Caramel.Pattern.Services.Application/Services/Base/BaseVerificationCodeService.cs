using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application.Services.Base
{
    [ExcludeFromCodeCoverage]
    public class BaseVerificationCodeService
    {
        protected string GenerateCode()
        {
            var random = new Random();
            const string chars = "1234567890";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
