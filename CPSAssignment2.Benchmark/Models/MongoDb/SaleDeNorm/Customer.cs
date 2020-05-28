using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    public class Customer
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        [BsonIgnoreIfNull]
        public int? SatisfactoryNumber { get; set; }
    }
}