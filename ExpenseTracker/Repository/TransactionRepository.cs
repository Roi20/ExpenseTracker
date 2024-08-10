﻿using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>,  ITransactionRepository
    {

        private readonly ICategoryRepository _categoryRepository;


        public TransactionRepository(ExpenseTrackerDbContext db, ICategoryRepository categoryRepository) : base(db)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PaginatedResult<Transaction>> GetPaginated(int page, int pageSize)
        {

            
            return await GetPaginated(page, pageSize);

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAll();
            }
            catch (Exception) 
            {
                throw;
            }
            
        }

    }
}
