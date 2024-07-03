using MoneyTransferAPI.Data;
using MoneyTransferAPI.Interfaces;
using MoneyTransferAPI.Models;
using MongoDB.Driver;

namespace MoneyTransferAPI.Services
{
    public class SavingService: ISavingService
    {
        private readonly MongoDBContext _context;
        public SavingService(MongoDBContext context)
        {
            _context = context;
        }
        public async Task<List<Saving>> GetSavingsByUserId(string userId)
        {
            var listOfSaving = await _context.Savings.Find(c => c.userId == userId).ToListAsync();
            foreach (var saving in listOfSaving)
            {
                if(saving.endDate <= DateTime.Now && saving.status == "processing")
                {
                    saving.status = "completed";
                    await _context.Savings.ReplaceOneAsync(c => c.Id == saving.Id, saving);
                }
            }
            return listOfSaving;
        }
        public async Task<Saving> CreateSaving(Saving saving, int months)
        {
            Saving newSaving = new Saving
            {
                userId = saving.userId,
                startDate = DateTime.Now,
                endDate = DateTime.Now.AddMonths(months),
                deposits = saving.deposits,
                moneyReceived = (int)(saving.deposits * 0.07 / 12 * months) + saving.deposits,
                status = "processing"
            };
            await _context.Savings.InsertOneAsync(newSaving);
            return newSaving;
        }
        public async Task<Saving> ChangeSavingStatus(string savingId)
        {
            var saving = await _context.Savings.Find(c => c.Id == savingId).FirstOrDefaultAsync();
            var user = await _context.Users.Find(c => c.Id == saving.userId).FirstOrDefaultAsync();
            if (saving.endDate <= DateTime.Now && saving.status == "completed")
            {
                user.money += saving.deposits + saving.moneyReceived;

                saving.status = "claimed";
            }
            else if(saving.endDate > DateTime.Now && saving.status == "processing")
            {
                user.money += saving.deposits;

                saving.status = "cancelled";
            }
            else
            {
                return null;
            }
            await _context.Users.ReplaceOneAsync(c => c.Id == saving.userId, user);
            await _context.Savings.ReplaceOneAsync(c => c.Id == savingId, saving);
            return saving;
        }

    }
}
