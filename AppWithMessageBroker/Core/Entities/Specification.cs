using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities
{
    public class Specification
    {
        [BsonElement("colour")]
        public string? Colour { get; set; }
        [BsonElement("quantity")]
        public Nullable<int> Quantity { get; set; }
        [BsonElement("isActive")]
        public Nullable<bool> IsActive { get; set; }
        [BsonElement("classification")]
        public string? Classification { get; set; }
        [BsonElement("instrumentClass")]
        public string? InstrumentClass { get; set; }
        [BsonElement("version")]
        public Nullable<decimal> Version { get; set; }
    }
}
