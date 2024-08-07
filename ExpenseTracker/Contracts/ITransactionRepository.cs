using System.Transactions;

namespace ExpenseTracker.Contracts
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
    }
}
