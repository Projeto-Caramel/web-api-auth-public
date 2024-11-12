using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum SheddingType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Baixa")]
        ShedsALot = 1,
        [Description("Sazional")]
        ShedsSeasonally = 2,
        [Description("Alta")]
        ShedsLittle = 3
    }
}
