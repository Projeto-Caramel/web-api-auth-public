using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum CoatType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Pelo Curto")]
        Short = 1,
        [Description("Pelo Longo")]
        Long = 2,
        [Description("Pelo Médio")]
        Medium = 3,
        [Description("Pouco Pelo")]
        Hairless = 4
    }
}
