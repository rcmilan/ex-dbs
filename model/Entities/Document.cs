using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace model.Entities
{
    public class Document : BaseEntity<ObjectId>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public override ObjectId ID { get; set; }

        [BsonElement("DateTime")]
        [Required]
        public override DateTime CreatedAt { get; set; }

        [BsonElement("Title")]
        [Required]
        public string Title { get; set; }

        [BsonElement("Subject")]
        [Required]
        public string Subject { get; set; }

        [BsonElement("IsValid")]
        [Required]
        public bool IsValid { get; set; }
    }
}