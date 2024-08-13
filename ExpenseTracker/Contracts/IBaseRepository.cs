﻿using ExpenseTracker.Common;
using System.Linq.Expressions;

namespace ExpenseTracker.Contracts
{
    public interface IBaseRepository<T>
    {
        Task <IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task Create(T entity);
        Task Update(object id, object model);
        Task Delete(object id);
        Task <PaginatedResult<T>> GetPaginated(int page, int pageSize, Expression<Func<T, bool>> condition);
        Task<PaginatedResult<T>> GetPagination(int page, int pageSize);

    }
}
