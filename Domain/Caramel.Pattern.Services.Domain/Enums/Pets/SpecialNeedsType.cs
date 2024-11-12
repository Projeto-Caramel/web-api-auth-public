using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum SpecialNeedsType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Sem necessidades especiais")]
        No = 1,
        [Description("Diabético")]
        Diabetic = 2,
        [Description("Cuidados Continuos")]
        ContinuousCare = 3
    }
}
