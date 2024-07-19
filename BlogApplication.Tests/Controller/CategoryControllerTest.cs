using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BlogApplication.Controllers;
using BlogApplication.Services.Categories;
using BlogApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using BlogApplication.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BlogApplication.Validators;

namespace BlogApplication.Tests.Controller
{
    public class CategoryControllerTest
    {

        [Fact]
        public async Task CategoryController_Index_ReturnsCorrectViewWithModel()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            var categories = new List<Category> { new Category(), new Category() };
            mockService.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(categories);

            var controller = new CategoryController(null, mockService.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());

            // Specify Moq.Times to avoid ambiguity
            mockService.Verify(service => service.GetCategoriesAsync(), Moq.Times.Once);
        }
        [Fact]
        public async Task CategoryController_Create_CreatesNewCategory_WhenNoExistingCategories()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            var categories = new List<Category>();
            mockService.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(categories);

            var controller = new CategoryController(null, mockService.Object);

            // Act
            var result = await controller.Create(new Category());

            // Assert
            var expectedCategoryId = 1; // Assuming the first ID is 1
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedCategory = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(expectedCategoryId, returnedCategory.CategoryId);
        }

        [Fact]
        public async Task CategoryController_Delete_ConfirmsDeletion_WhenCategoryExists()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            var category = new Category { CategoryId = 1 };
            mockService.Setup(service => service.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
            mockService.Setup(service => service.DeleteCategoryByIdAsync(It.IsAny<int>())).Callback<int>(id => Assert.Equal(category.CategoryId, id));

            var controller = new CategoryController(null, mockService.Object);

            // Act
            await controller.DeleteConfirmed(category.CategoryId);

            // Assert
            mockService.Verify(service => service.DeleteCategoryByIdAsync(It.IsAny<int>()), Moq.Times.Once); // Specify Moq.Times
        }
        [Fact]
        public async Task CategoryController_Details_ReturnsCorrectViewWithModel()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            var category = new Category { CategoryId = 1 }; // Example category
            mockService.Setup(service => service.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(category);

            var controller = new CategoryController(null, mockService.Object);

            // Act
            var result = await controller.Details(1); // Pass the ID of the category we set up

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(category, model);
        }
























    }
}
