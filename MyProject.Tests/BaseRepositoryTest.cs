using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using ExpenseTracker.Common;
using System.Linq.Expressions;
using ExpenseTracker.Repository;
using Microsoft.AspNetCore.Authentication;
using NuGet.Protocol.Core.Types;
using ExpenseTracker.Context;
using System.Numerics;

namespace MyProject.Testing
{

    [TestFixture]
    public class MyTest
    {
        [Test]
        public void Test()
        {
            ClassicAssert.AreEqual(1, 1);
        }
    }


    /*
    public class BaseRepositoryTest<T>
        where T : class, IBaseModel
    {

        private readonly DbContext _context;
        private readonly DbSet<T> _entityTable;

        public BaseRepositoryTest(DbContext context)
        {
            _context = context; 
            _entityTable = _context.Set<T>();
        }


        public void AddEntity(T entity)
        {
            _entityTable.Add(entity);
        }

        public T GetById(int id)
        {
            return _entityTable.Find(id);
        }

        public List<T> GetAll() 
        {

            return _entityTable.ToList();

        }

        public void Update(object id, object model)
        {

            var entity = GetById((int)id);

            _context.Entry(entity).CurrentValues.SetValues(model);
            _context.SaveChanges();

        }
        public void Delete(int id)
        {
            var entity = GetById(id);

            _entityTable.Remove(entity);
            _context.SaveChanges();
        }



    }

    public class PaginatedResult<T>
    {
        public List<T> Result { get; set; }
        public int Page { get; set; }
        public int TotalCount { get; set; }
    }


  
    public class ExpenseTrackerDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }

    }

    [TestFixture]
    public class BaseRepoTest
    {
        private ExpenseTrackerDbContext _context;
        private BaseRepositoryTest<Category> _repo;
        private BaseRepository<Category> _repository;
        private DbContextOptions<ExpenseTrackerDbContext> _options;

        [SetUp]
        public void setup()
        {
      
            _context = new ExpenseTrackerDbContext();
            _repo = new BaseRepositoryTest<Category>(_context);

        }

        [Test]
        public void AddEntity()
        {
            var category = new Category {CategoryId = 1, Icon = "i", Title = "Mytitle", Type = "Income", User_Id = "Sample UserId" };

            _repo.AddEntity(category);
            _context.SaveChanges();

            var addedCategory = _context.Categories.Find(1);

            ClassicAssert.IsNotNull(addedCategory, "Category added");
            ClassicAssert.AreEqual(category.Title, addedCategory?.Title, "Title should be match");

            Console.WriteLine("My Test");

        }

        [Test]
        public void GetId_ShouldReturnCorrectEntity()
        {
            var categories = new List<Category>
            {
                new Category {CategoryId = 1, Icon = "i", Title = "title 1", Type = "Income 1", User_Id = "Sample UserId1" },
                new Category {CategoryId = 2, Icon = "i", Title = "title 2", Type = "Income 2", User_Id = "Sample UserId2" },
                new Category {CategoryId = 3, Icon = "i", Title = "title 3", Type = "Income 3", User_Id = "Sample UserId3" },
            };


            _context.Categories.AddRange(categories);
            _context.SaveChanges();


            var entity = _repo.GetById(3);

            ClassicAssert.AreEqual(3, entity.CategoryId, "CategoryId should match");
            ClassicAssert.IsNotNull(entity, "Entity should be populated");
            ClassicAssert.AreEqual("title 3", entity.Title, "titles should be match");
        }

        [Test]
        public void GetAll()
        {

            var categories = new List<Category>
            {
                new Category {CategoryId = 1, Icon = "i", Title = "title 1", Type = "Income 1", User_Id = "Sample UserId1" },
                new Category {CategoryId = 2, Icon = "i", Title = "title 2", Type = "Income 2", User_Id = "Sample UserId2" },
                new Category {CategoryId = 3, Icon = "i", Title = "title 3", Type = "Income 3", User_Id = "Sample UserId3" },
            };


            _context.Categories.AddRange(categories);
            _context.SaveChanges();


            var result = _repo.GetAll();

            ClassicAssert.AreEqual(3, result.Count());
            ClassicAssert.AreEqual("title 1", result[0].Title);
            ClassicAssert.AreEqual("title 2", result[1].Title);
            ClassicAssert.AreEqual("title 3", result[2].Title);
        }


        [Test]
        public void UpdateEntity()
        {

            var categories = new List<Category>
            {
                new Category {CategoryId = 1, Icon = "i", Title = "title 1", Type = "Income 1", User_Id = "Sample UserId1" },
                new Category {CategoryId = 2, Icon = "i", Title = "title 2", Type = "Income 2", User_Id = "Sample UserId2" },
                new Category {CategoryId = 3, Icon = "i", Title = "title 3", Type = "Income 3", User_Id = "Sample UserId3" },
            };


            _context.Categories.AddRange(categories);
            _context.SaveChanges();

          

            _repo.Update(1, new {Title = "Jumong", User_Id = "MyUserId"});

            var result = _repo.GetAll();


            ClassicAssert.AreEqual("Jumong", result[0].Title);
            ClassicAssert.AreEqual("MyUserId", result[0].User_Id);

        }

        [Test]
        public void DeleteEntity() 
        {

            var categories = new List<Category>
            {
                new Category {CategoryId = 1, Icon = "i", Title = "title 1", Type = "Income 1", User_Id = "Sample UserId1" },
                new Category {CategoryId = 2, Icon = "i", Title = "title 2", Type = "Income 2", User_Id = "Sample UserId2" },
                new Category {CategoryId = 3, Icon = "i", Title = "title 3", Type = "Income 3", User_Id = "Sample UserId3" },
            };

            _context.Categories.AddRange(categories);
            _context.SaveChanges();


            _repo.Delete(1);
            _repo.Delete(3);

            var result = _repo.GetAll();

            ClassicAssert.AreEqual(1, result.Count());
            ClassicAssert.IsNull(_context.Categories.Find(1));
            ClassicAssert.IsNull(_context.Categories.Find(3));

        }


        [Test]
        public void Pagination()
        {
            
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

    */

    [TestFixture]
    public class TestBaseRepository
    {
        private ExpenseTrackerDbContext _context;
        private DbContextOptions<ExpenseTrackerDbContext> _options;
        private BaseRepository<Category> _repo;
        private CategoryRepository repo;


        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExpenseTrackerDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestDataBase");

            _options = optionsBuilder.Options;
            _context = new ExpenseTrackerDbContext(_options);
            _repo = new BaseRepository<Category>(_context);
            repo = new CategoryRepository(_context);

        }

        [Test]
        public void TestCalculator()
        {
            var a = 10;
            var b = 11;

            var result = Calculator(a, b);

            ClassicAssert.AreEqual(21, result);


        }


        [Test]
        public void TestDbContext()
        {
            ClassicAssert.IsNotNull(_context);
        }

        [Test]
        public async Task AddEntity_ShouldAddAnEntity()
        {
           
            var category = new Category
            {
                CategoryId = 1,
                Title = "My Title",
                Icon = "Icon",
                Type = "Type 1",
                User_Id = "1234"
            };

            await _repo.Create(category);

            var result = await _repo.GetAll(category.User_Id);
            var resToList = result.ToList();

            ClassicAssert.AreEqual(1, result.Count());
            ClassicAssert.AreEqual("1234", resToList[0].User_Id);
            
        }
        
        [Test]
        public async Task GetPagination_ShouldReturnPaginatedPage()
        {


            var category1 = new Category { CategoryId = 1, Icon = "i", Title = "title 1", Type = "Income 1", User_Id = "Sample UserId1" };
            var category2 = new Category { CategoryId = 2, Icon = "i", Title = "title 1", Type = "Income 2", User_Id = "Sample UserId1" };
            var category3 = new Category { CategoryId = 3, Icon = "i", Title = "title  6", Type = "Income 3", User_Id = "Sample UserId1" };
            var category4 = new Category { CategoryId = 4, Icon = "i", Title = "titless 6", Type = "Income 4", User_Id = "Sample UserId1" };
            var category5 = new Category { CategoryId = 5, Icon = "i", Title = "title 6", Type = "Income 5", User_Id = "Sample UserId1" };
            var category6 = new Category { CategoryId = 6, Icon = "i", Title = "title 6", Type = "Income 6", User_Id = "Sample UserId1" };


            await _repo.Create(category1);
            await _repo.Create(category2);
            await _repo.Create(category3);
            await _repo.Create(category4);
            await _repo.Create(category5);
            await _repo.Create(category6);

            /*
            int page = 1;
            int pageSize = 2;
            string keyword = "1";
            Expression<Func<Category, bool>> condition = x => x.Title.Contains(keyword ?? string.Empty);
            var userId = "Sample UserId1";


           // var result = await repo.GetPaginated(page, pageSize, keyword, userId);


            // number of pages based on the pagesize, if the table has 6 entities
            // TotalCount = (int)MathF.Ceiling(count / (double)pageSize)
            //Note: check the keyword, the total count will be changed depends on keyword.
            ClassicAssert.AreEqual(1, result.TotalCount);

            //item that present on the current page, was the only item will be counted.
            ClassicAssert.AreEqual(2, result.Result.Count());
         */
        }
            

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        public static int Calculator(int a, int b)
        {
            return a + b;
        }


        public class Pagination : PaginatedResult<Category>
        {

        }
        
    }



}







