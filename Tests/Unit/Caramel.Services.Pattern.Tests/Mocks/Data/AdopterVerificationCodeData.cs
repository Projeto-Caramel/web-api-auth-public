using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public static class AdopterVerificationCodeData
    {
        public static Dictionary<string, AdopterVerificationCode> Data = new()
        {
            {
                "Basic", new AdopterVerificationCode()
                {
                    Id = "t35t3",
                    Code = "123456",
                    UserEmail = "t35t3",
                    ValidTo = DateTime.Now.AddMinutes(10)
                }
            },
            {
                "Invalid", new AdopterVerificationCode()
                {
                    Id = "t35t3",
                    Code = "654321",
                    UserEmail = "t35t3",
                    ValidTo = DateTime.Now.AddMinutes(-10)
                }
            },
        };
    }
}
