using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums.Pets
{
    public enum AgeType
    {
        [Description("Indefinida")]
        Unknown = 0,
        [Description("Filhote")]
        Puppy = 1,
        [Description("Jovem")]
        Young = 2,
        [Description("Adulto")]
        Adult = 3,
        [Description("Idoso")]
        Senior = 4
    }
}
