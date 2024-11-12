using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum StimulusLevelType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Baixo")]
        Low = 1,
        [Description("Moderado")]
        Moderate = 2,
        [Description("Alto")]
        High = 3
    }
}
