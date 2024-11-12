using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class PartnerUpdatePasswordRequestData
    {
        public static Dictionary<string, PartnerUpdatePasswordApiRequest> Data = new()
        {
            {
                "Basic", new PartnerUpdatePasswordApiRequest()
                {
                    Name = "Basic",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Role = 0,
                    Email = "test@basic.com"
                }
            },
            {
                "InvalidPassword", new PartnerUpdatePasswordApiRequest()
                {
                    Name = "Invalid",
                    Password = "XW+cxE2WUFQMkjEKgwT2Hw==",
                    Role = 0,
                    Email = "test@basic.com"
                }
            },
            {
                "BasicUpdate", new PartnerUpdatePasswordApiRequest()
                {
                    Name = "Basic Update",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Role = 0,
                    Email = "test@basic-update.com"
                }
            },
            {
                "UpdateException", new PartnerUpdatePasswordApiRequest()
                {
                    Name = "Update exception",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Role = 0,
                    Email = "test@update-exception.com"
                }
            },
            { "Empty", new PartnerUpdatePasswordApiRequest() },
            { "WithoutId", new PartnerUpdatePasswordApiRequest() },
            { "Null", null },
        };
    }
}
