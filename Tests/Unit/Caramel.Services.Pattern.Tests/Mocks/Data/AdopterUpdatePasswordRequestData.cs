using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Adopters;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class AdopterUpdatePasswordRequestData
    {
        public static Dictionary<string, AdopterUpdatePasswordApiRequest> Data = new()
        {
            {
                "Basic", new AdopterUpdatePasswordApiRequest()
                {
                    NewPassword = "dTy7m726FLaYCQWMdzOYTg==",
                    Email = "test@basic.com"
                }
            },
            {
                "InvalidPassword", new AdopterUpdatePasswordApiRequest()
                {
                    NewPassword = "XW+cxE2WUFQMkjEKgwT2Hw==",
                    Email = "test@basic.com"
                }
            },
            {
                "BasicUpdate", new AdopterUpdatePasswordApiRequest()
                {
                    NewPassword = "dTy7m726FLaYCQWMdzOYTg==",
                    Email = "test@basic-update.com"
                }
            },
            {
                "UpdateException", new AdopterUpdatePasswordApiRequest()
                {
                    NewPassword = "dTy7m726FLaYCQWMdzOYTg==",
                    Email = "test@update-exception.com"
                }
            },
            { "Empty", new AdopterUpdatePasswordApiRequest() },
            { "WithoutId", new AdopterUpdatePasswordApiRequest() },
            { "Null", null },
        };
    }
}
