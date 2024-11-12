using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Adopters
{
    public enum FinancialSituation
    {
        [Description("Indefinido")]
        Blank,
        [Description("Confortável")]
        Confortable,
        [Description("Normal")]
        Normal,
        [Description("Instável")]
        Careful,
        [Description("Dificuldade Financeira")]
        Difficult
    }
}
