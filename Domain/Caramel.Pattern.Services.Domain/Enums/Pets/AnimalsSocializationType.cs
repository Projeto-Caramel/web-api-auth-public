using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum AnimalsSocializationType
    {
        [Description("Indefinido")]
        Unknown = 0,
        [Description("Prefere ser o único pet")] 
        OnlyPet = 1,
        [Description("Somente mesmas espécies")]
        SameSpeciesOnly = 2,
        [Description("Amigável com outros animais")]
        Friendly = 3
    }
}
