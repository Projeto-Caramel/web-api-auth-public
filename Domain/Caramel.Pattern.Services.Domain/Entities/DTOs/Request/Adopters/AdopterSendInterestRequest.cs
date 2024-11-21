using Caramel.Pattern.Services.Domain.Enums.Adopters;
using Caramel.Pattern.Services.Domain.Enums.Pets;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters
{
    public class AdopterSendInterestRequest
    {
        public AdopterInfos AdopterInfos { get; set; }
        public PetInfos PetInfos { get; set; }
        public string PartnerId { get; set; }
        
    }

    public class AdopterInfos
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AdopterPhone { get; set; }
        public DateOnly Birthday { get; set; }
        public ResidencyType ResidencyType { get; set; }
        public Lifestyle Lifestyle { get; set; }
        public PetExperience PetExperience { get; set; }
        public HasChildren HasChildren { get; set; }
        public FinancialSituation FinancialSituation { get; set; }
        public FreeTime FreeTime { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class PetInfos
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BirthDate { get; set; }
        public PetStatus Status { get; set; }
        public PetSex Sex { get; set; }
        public CoatType Coat { get; set; }
        public EnergyLevelType EnergyLevel { get; set; }
        public SizeType Size { get; set; }
        public StimulusLevelType StimulusLevel { get; set; }
        public TemperamentType Temperament { get; set; }
        public ChildLoveType ChildLove { get; set; }
        public AnimalsSocializationType AnimalsSocialization { get; set; }
        public SpecialNeedsType SpecialNeeds { get; set; }
        public SheddingType Shedding { get; set; }
    }
}
