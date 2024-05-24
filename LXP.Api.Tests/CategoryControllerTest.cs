using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using System.Net;
using LXP.Api.Controllers;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryServices> _categoryServicesMock;
        private CategoryController _categoryController;

        [SetUp]
        public void Setup()
        {
            _categoryServicesMock = new Mock<ICategoryServices>();
            _categoryController = new CategoryController(_categoryServicesMock.Object);
        }

        [Test]
        public async Task GetAllCategory_ReturnsOkResultWithCategories()
        {
            // Arrange
            var categories = new List<CourseCategoryListViewModel>
            {
                new CourseCategoryListViewModel { CatagoryId = Guid.NewGuid(), Category = "Category 1" },
                new CourseCategoryListViewModel { CatagoryId = Guid.NewGuid(), Category = "Category 2" }
            };

            _categoryServicesMock.Setup(x => x.GetAllCategory()).Returns(Task.FromResult(categories));

            // Act
            var result = await _categoryController.GetAllCategory();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            APIResponse apiResponse = (APIResponse)okResult.Value;
            Assert.AreEqual((int)HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual(categories, apiResponse.Data);
        }

        [Test]
        public async Task AddCategory_NewCategory_ReturnsOkResultWithInsertedCategory()
        {
            // Arrange
            var categoryViewModel = new CourseCategoryViewModel { Category = "New Category" };
            var insertedCategory = new CourseCategoryListViewModel { CatagoryId = Guid.NewGuid(), Category = "New Category" };

            _categoryServicesMock.Setup(x => x.AddCategory(categoryViewModel)).Returns(Task.FromResult(true));
            _categoryServicesMock.Setup(x => x.GetCategoryByCategoryName(categoryViewModel.Category)).Returns(Task.FromResult(insertedCategory));

            // Act
            var result = await _categoryController.AddCategory(categoryViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            APIResponse apiResponse = (APIResponse)okResult.Value;
            Assert.AreEqual((int)HttpStatusCode.Created, apiResponse.StatusCode);
            Assert.AreEqual(insertedCategory, apiResponse.Data);
        }

        [Test]
        public async Task AddCategory_ExistingCategory_ReturnsOkResultWithFailureMessage()
        {
            // Arrange
            var categoryViewModel = new CourseCategoryViewModel { Category = "Existing Category" };

            _categoryServicesMock.Setup(x => x.AddCategory(categoryViewModel)).Returns(Task.FromResult(false));

            // Act
            var result = await _categoryController.AddCategory(categoryViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            APIResponse apiResponse = (APIResponse)okResult.Value;
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, apiResponse.StatusCode);
            Assert.AreEqual(MessageConstants.MsgAlreadyExists, apiResponse.Message);
        }
    }
}