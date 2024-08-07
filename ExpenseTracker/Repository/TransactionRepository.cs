using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using System.Transactions;

namespace ExpenseTracker.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ExpenseTrackerDbContext db) : base(db)
        {
         
        }


        


    }
}
