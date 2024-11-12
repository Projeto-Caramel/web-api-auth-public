using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Adopters
{
    public enum FreeTime
    {
        [Description("Indefinido")]
        Blank,
        [Description("Muito Pouco")]
        Little,
        [Description("Pouco")]
        Low,
        [Description("Médio")]
        Medium,
        [Description("Muito")]
        High
    }
}
