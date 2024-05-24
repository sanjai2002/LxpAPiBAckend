using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Core.Services;

namespace LXP.Core.Tests.Services
{
    [TestFixture]
    public class CategoryServicesTests
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private CategoryServices _categoryServices;

        [SetUp]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryServices = new CategoryServices(_categoryRepositoryMock.Object);
        }

        [Test]
        public async Task AddCategory_CategoryDoesNotExist_ReturnsTrue()
        {
            // Arrange
            var categoryViewModel = new CourseCategoryViewModel
            {
                Category = "New Category",
                CreatedBy = "Admin",

            };

            _categoryRepositoryMock.Setup(x => x.AnyCategoryByCategoryName(It.IsAny<string>())).Returns(Task.FromResult(false));

            // Act
            var result = await _categoryServices.AddCategory(categoryViewModel);

            // Assert
            Assert.IsTrue(result);
            _categoryRepositoryMock.Verify(x => x.AddCategory(It.IsAny<CourseCategory>()), Times.Once);
        }

        [Test]
        public async Task AddCategory_CategoryExists_ReturnsFalse()
        {
            // Arrange
            var categoryViewModel = new CourseCategoryViewModel
            {
                Category = "Existing Category",
                CreatedBy = "Admin"
            };

            _categoryRepositoryMock.Setup(x => x.AnyCategoryByCategoryName(It.IsAny<string>())).Returns(Task.FromResult(true));

            // Act
            var result = await _categoryServices.AddCategory(categoryViewModel);

            // Assert
            Assert.IsFalse(result);
            _categoryRepositoryMock.Verify(x => x.AddCategory(It.IsAny<CourseCategory>()), Times.Never);
        }

        [Test]
        public async Task GetAllCategory_ReturnsListOfCategories()
        {
            // Arrange
            var categories = new List<CourseCategory>
            {
                new CourseCategory { CatagoryId = Guid.NewGuid(), Category = "Category 1", CreatedAt = DateTime.Now, CreatedBy = "Admin" },
                new CourseCategory { CatagoryId = Guid.NewGuid(), Category = "Category 2", CreatedAt = DateTime.Now, CreatedBy = "Admin" }
            };

            _categoryRepositoryMock.Setup(x => x.GetAllCategory()).Returns(Task.FromResult(categories));

            // Act
            var result = await _categoryServices.GetAllCategory();

            // Assert
            Assert.AreEqual(categories.Count, result.Count);
        }

        [Test]
        public async Task GetCategoryByCategoryName_ReturnsCategory()
        {
            // Arrange
            var categoryName = "Category 1";
            var category = new CourseCategory { CatagoryId = Guid.NewGuid(), Category = categoryName, CreatedAt = DateTime.Now, CreatedBy = "Admin" };

            _categoryRepositoryMock.Setup(x => x.GetCategoryByCategoryName(categoryName)).Returns(category);

            // Act
            var result = await _categoryServices.GetCategoryByCategoryName(categoryName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(category.Category, result.Category);
        }
    }
}