using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MoneyTransferAPI.Models
{
    public class Transaction
    {
        [BsonId]
        //[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("userSendId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userSendId { get; set; } = null!;


        [BsonElement("userReceivedId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userReceivedId { get; set; } = null!;


        [BsonElement("money")]
        public int money { get; set; }


        [BsonElement("date")]
        public DateTime date { get; set; }        

    }
}
