using MoneyTransferAPI.Models;

namespace MoneyTransferAPI.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactionsByUserId(string userSendId);
        Task<Transaction> CreateTransaction(Transaction transaction);
    }
}
