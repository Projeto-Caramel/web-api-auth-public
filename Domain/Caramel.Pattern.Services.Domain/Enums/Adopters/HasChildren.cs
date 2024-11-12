using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Adopters
{
    public enum HasChildren
    {
        [Description("Indefinido")]
        Blank,
        [Description("Sim")]
        Yes,
        [Description("Não")]
        No
    }
}
