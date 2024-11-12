using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public static class PartnerVerificationCodeData
    {
        public static Dictionary<string, PartnerVerificationCode> Data = new Dictionary<string, PartnerVerificationCode>
        {
            {
                "Basic", new PartnerVerificationCode()
                {
                    Id = "t35t3",
                    Code = "123456",
                    PartnerId = "t35t3",
                    ValidTo = DateTime.Now.AddMinutes(10)
                }
            },
            {
                "Invalid", new PartnerVerificationCode()
                {
                    Id = "t35t3",
                    Code = "654321",
                    PartnerId = "t35t3",
                    ValidTo = DateTime.Now.AddMinutes(-10)
                }
            },
        };
    }
}
