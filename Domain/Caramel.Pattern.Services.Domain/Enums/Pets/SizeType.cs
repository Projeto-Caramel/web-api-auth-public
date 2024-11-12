using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum SizeType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Pequeno")]
        Small = 1,
        [Description("Médio")]
        Medium = 2,
        [Description("Grande")]
        Large = 3
    }
}
