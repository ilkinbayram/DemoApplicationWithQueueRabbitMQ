using Core.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities
{
    public class Product : IEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("productName")]
        public string? ProductName { get; set; }


        [BsonElement("model")]
        public string? Model { get; set; }

        [BsonElement("genre")]
        public string? Genre { get; set; }


        [BsonElement("baseCategory")]
        public string? BaseCategory { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }

        [BsonElement("price")]
        public Nullable<decimal> Price { get; set; }

        [BsonElement("year")]
        public Nullable<int> Year { get; set; }

        [BsonElement("specifications")]
        public IEnumerable<Specification>? Specifications { get; set; }
    }
}
