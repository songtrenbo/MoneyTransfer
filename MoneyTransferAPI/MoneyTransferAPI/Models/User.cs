using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MoneyTransferAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [Required]
        [BsonElement("name")]
        public string name { get; set; } = null!;


        [Required]
        [BsonElement("creditCardId")]
        public string creditCardId { get; set; } = null!;


        [Required]
        [BsonElement("creditCardName")]
        public string creditCardName { get; set; } = null!;


        [Required]
        [BsonElement("username")]
        public string username { get; set; } = null!;


        [Required]
        [BsonElement("password")]
        public string password { get; set; } = null!;



        [Required]
        [BsonElement("money")]
        public int money { get; set; } 
    }
}
