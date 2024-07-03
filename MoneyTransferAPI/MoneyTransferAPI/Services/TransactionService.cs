using MoneyTransferAPI.Data;
using MoneyTransferAPI.Interfaces;
using MoneyTransferAPI.Models;
using MongoDB.Driver;

namespace MoneyTransferAPI.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly MongoDBContext _context;
        public TransactionService(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsByUserId(string userSendId)
        {
            return await _context.Transactions.Find(c=>c.userSendId==userSendId).ToListAsync();
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        { 
            Transaction tran = new Transaction
            {
                userSendId = transaction.userSendId,
                userReceivedId = transaction.userReceivedId,
                money = transaction.money,
                date = DateTime.Now,
            };
            await _context.Transactions.InsertOneAsync(tran);
            return tran;
        }
    }
}
