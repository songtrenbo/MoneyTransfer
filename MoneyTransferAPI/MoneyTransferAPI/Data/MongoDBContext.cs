using MoneyTransferAPI.Models;
using MongoDB.Driver;

namespace MoneyTransferAPI.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("transactions");
        public IMongoCollection<Saving> Savings => _database.GetCollection<Saving>("savings");

    }
}
