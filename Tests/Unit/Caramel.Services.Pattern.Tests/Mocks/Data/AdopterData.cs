using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class AdopterData
    {
        public static Dictionary<string, Adopter> Data = new()
        {
            {
                "Basic", new Adopter()
                {
                    Id = "t35t3",
                    Name = "Basic",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Email = "test@basic.com"
                }
            },
            {
                "InvalidPassword", new Adopter()
                {
                    Id = "t35t3",
                    Name = "Invalid",
                    Password = "XW+cxE2WUFQMkjEKgwT2Hw==",
                    Email = "test@basic.com",
                }
            },
            {
                "BasicUpdate", new Adopter()
                {
                    Id = "t35t3",
                    Name = "Basic Update",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Email = "test@basic-update.com",
                }
            },
            {
                "UpdateException", new Adopter()
                {
                    Id = "t35t3-3rr0r",
                    Name = "Update exception",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Email = "test@update-exception.com",
                }
            },
            { "Empty", new Adopter() { Id = "t35t3" } },
            { "WithoutId", new Adopter() },
            { "Null", null },
        };
    }
}
