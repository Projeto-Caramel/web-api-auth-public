using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum EnergyLevelType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Pouca Energia")]
        Low = 1,
        [Description("Média Energia")]
        Medium = 2,
        [Description("Muita Energia")]
        High = 3,
    }
}
