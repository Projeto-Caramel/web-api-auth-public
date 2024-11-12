using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum TemperamentType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Não amigável")]
        NotFriendly = 1,
        [Description("Um pouco Amigável")]
        ShyFriendly = 2,
        [Description("Muito Amigáve")]
        VeryFriendly = 3
    }
}
