using Caramel.Pattern.Services.Domain.Enums.Adopters;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Adopters
{
    [ExcludeFromCodeCoverage]
    public class AdopterUpdateApiRequest
    {
        public string Name { get; set; }
        public DateOnly Birthday { get; set; }
        public ResidencyType ResidencyType { get; set; }
        public Lifestyle Lifestyle { get; set; }
        public PetExperience PetExperience { get; set; }
        public HasChildren HasChildren { get; set; }
        public FinancialSituation FinancialSituation { get; set; }
        public FreeTime FreeTime { get; set; }
    }
}
