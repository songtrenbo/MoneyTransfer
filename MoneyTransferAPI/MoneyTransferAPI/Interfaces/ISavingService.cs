using MoneyTransferAPI.Models;

namespace MoneyTransferAPI.Interfaces
{
    public interface ISavingService
    {
        Task<List<Saving>> GetSavingsByUserId(string userId);
        Task<Saving> CreateSaving(Saving saving, int months);
        Task<Saving> ChangeSavingStatus(string savingId);
    }
}
