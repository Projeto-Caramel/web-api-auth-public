using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerVerificationCode : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public string Code { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
