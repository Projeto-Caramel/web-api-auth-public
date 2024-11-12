using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Adopters
{
    public enum Lifestyle
    {
        [Description("Indefinido")]
        Blank,
        [Description("Muito Ativo")]
        VeryActive,
        [Description("Moderadamente Ativo")]
        ModeratelyActive,
        [Description("Ligeiramente Ativo")]
        SlightlyActive,
        [Description("Sedentário")]
        Sedentary
    }
}
