using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Adopters
{
    public enum ResidencyType
    {
        [Description("Indefinido")]
        Blank,
        [Description("Casa")]
        House,
        [Description("Apartamento")]
        Apartment,
        [Description("Chácara ou Sítio")]
        Farm
    }
}
