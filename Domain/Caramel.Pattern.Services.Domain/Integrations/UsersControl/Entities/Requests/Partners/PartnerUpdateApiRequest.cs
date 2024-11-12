using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerUpdateApiRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public string CNPJ { get; set; }
        public string AdoptionRate { get; set; }
        public string PIX { get; set; }
        public string Website { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
    }
}
