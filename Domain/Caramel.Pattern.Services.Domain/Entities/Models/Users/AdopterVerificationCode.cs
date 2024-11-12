using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Users
{
    [ExcludeFromCodeCoverage]
    public class AdopterVerificationCode : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserEmail { get; set; }
        public string Code { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
