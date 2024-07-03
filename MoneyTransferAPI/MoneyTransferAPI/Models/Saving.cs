using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyTransferAPI.Models
{
    public class Saving
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; } = null!;




        [BsonElement("startDate")]
        public DateTime startDate { get; set; }


        [BsonElement("endDate")]
        public DateTime endDate { get; set; }


        [BsonElement("deposits")]
        public int deposits { get; set; }


        [BsonElement("moneyReceived")]
        public int moneyReceived { get; set; }


        [BsonElement("status")]
        public string status { get; set; }
    }
}
