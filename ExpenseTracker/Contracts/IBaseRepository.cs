namespace ExpenseTracker.Contracts
{
    public interface IBaseRepository<T>
    {
        Task <IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Create(T entity);
        Task Update(object id, object model);
        Task Delete(object id);


        //Get all by user id - Not yet implemented, this is just a provision for integrating user input data
        Task<IEnumerable<T>> UserId(string userId);



    }
}
