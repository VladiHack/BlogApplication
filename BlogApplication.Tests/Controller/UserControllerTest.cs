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

    public class UserControllerTest
    {

        [Fact]
        public async Task UserController_Index_ReturnsViewResult_WithListOfUsers()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUsersAsync()).ReturnsAsync(new List<User>());

            var controller = new UserController(null, mockUserService.Object);

            // Act
            var result = await controller.Index(); // Await the asynchronous call

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Now result is of type IActionResult
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task UserController_Details_ReturnsCorrectViewWithModel()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var user = new User { UserId = 1, Username = "TestUser" };
            var users = new List<User> { user };
            mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var controller = new UserController(null, mockUserService.Object);

            // Act
            var result = await controller.DetailsAsync(user.UserId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal(user, model);
        }
        [Fact]
        public async Task UserController_DeleteConfirmed_DeletesUser_WhenUserExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var user = new User { UserId = 1, Username = "TestUser" };
            mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            mockUserService.Setup(service => service.DeleteUserByIdAsync(It.IsAny<int>())).Callback<int>(id => Assert.Equal(user.UserId, id));

            var controller = new UserController(null, mockUserService.Object);

            // Act
            await controller.DeleteConfirmed(user.UserId);

            // Assert
            mockUserService.Verify(service => service.DeleteUserByIdAsync(It.IsAny<int>()), Moq.Times.Once);
        }
        [Fact]
        public void UserController_Create_ReturnsViewResult()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(null, mockUserService.Object);

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }


      






    }
}
