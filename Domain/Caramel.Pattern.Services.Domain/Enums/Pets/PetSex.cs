using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum PetSex
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Macho")]
        Male = 1,
        [Description("Fêmea")]
        Female = 2
    }
}
