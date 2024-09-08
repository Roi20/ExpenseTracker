using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
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

namespace MyProject.Tests
{
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

    public class TestDbContext : DbContext
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
        private TestDbContext _context;
        private BaseRepositoryTest<Category> _repo;

        [SetUp]
        public void setup()
        {
            _context = new TestDbContext();
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

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

}


