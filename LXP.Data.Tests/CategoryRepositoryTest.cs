using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LXP.Data.Repository.Tests
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private DbContextOptions<LXPDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "lxp")
                .Options;

            using (var context = new LXPDbContext(_options))
            {
                context.CourseCategories.AddRange(

                    new CourseCategory
                    {
                        CatagoryId = Guid.NewGuid(),
                        Category = "Category 1",
                        CreatedBy = "Karni",
                        CreatedAt = DateTime.Now,
                        ModifiedBy = "karikalan",
                        ModifiedAt = DateTime.Now
                    },
                    new CourseCategory
                    {
                        CatagoryId = Guid.NewGuid(),
                        Category = "Category 2",
                        CreatedBy = "Karni",
                        CreatedAt = DateTime.Now,
                        ModifiedBy = "karikalan",
                        ModifiedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }


        [Test]
        public async Task AddCategory_ValidCategory_AddsCategoryToContextAndSavesChanges()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CategoryRepository(context);
                var categoryToAdd = new CourseCategory
                {
                    CatagoryId = Guid.NewGuid(),
                    Category = "Category 2",
                    CreatedBy = "Karni",
                    CreatedAt = DateTime.Now,
                    ModifiedBy = "karikalan",
                    ModifiedAt = DateTime.Now
                };


                // Act
                await repository.AddCategory(categoryToAdd);

                // Assert
                var addedCategory = await context.CourseCategories.FindAsync(categoryToAdd.CatagoryId);
                Assert.IsNotNull(addedCategory);
                Assert.AreEqual(categoryToAdd.Category, addedCategory.Category);
            }
        }

        //[Test]
        //public async Task GetAllCategory_ReturnsAllCategoriesOrderedByName()
        //{
        //    // Arrange
        //    using (var context = new LXPDbContext(_options))
        //    {
        //        var repository = new CategoryRepository(context);

        //        // Act
        //        dynamic result = repository.GetAllCategory();

        //        // Assert
        //        Assert.AreEqual(2, result.Length);
        //        Assert.AreEqual("Category 1", result[0].Category);
        //        Assert.AreEqual("Category 2", result[1].Category);
        //    }
        //}

        [Test]
        public async Task AnyCategoryByCategoryName_ExistingCategory_ReturnsTrue()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act
                var result = await repository.AnyCategoryByCategoryName("Category 1");

                // Assert
                Assert.IsTrue(result);
            }
        }

        [Test]
        public async Task AnyCategoryByCategoryName_NonExistingCategory_ReturnsFalse()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act
                var result = await repository.AnyCategoryByCategoryName("Non-existing Category");

                // Assert
                Assert.IsFalse(result);
            }
        }

        [Test]
        public async Task GetCategoryByCategoryId_ExistingCategoryId_ReturnsCategory()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CategoryRepository(context);
                var categoryId = context.CourseCategories.First().CatagoryId;

                // Act
                var result = repository.GetCategoryByCategoryId(categoryId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(categoryId, result.CatagoryId);
            }
        }

        [Test]
        public void GetCategoryByCategoryName_ExistingCategoryName_ReturnsCategory()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act
                var result = repository.GetCategoryByCategoryName("Category 1");

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Category 1", result.Category);
            }
        }

        [Test]
        public void GetCategoryByCategoryName_NonExistingCategoryName_ThrowsException()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act and Assert
                Assert.Throws<InvalidOperationException>(() => repository.GetCategoryByCategoryName("Non-existing Category"));
            }
        }
    }
}