using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum ChildLoveType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Não amigável com crianças")]
        No = 1,
        [Description("Tolerante a crianças")]
        Tolerant = 2,
        [Description("Ama crianças")]
        Loves = 3
    }
}
