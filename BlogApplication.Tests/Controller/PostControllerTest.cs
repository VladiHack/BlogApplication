using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BlogApplication.Controllers;
using BlogApplication.Services.Posts;
using BlogApplication.Services.Categories;
using BlogApplication.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlogApplication.Validators;
using BlogApplication.Services.Users;
using System.Linq.Expressions;

namespace BlogApplication.Tests.Controller
{
    public class PostControllerTest
    {
        private readonly Mock<BlogDbContext> _dbContextMock;
        private readonly Mock<IPostsService> _postServiceMock;
        private readonly Mock<ICategoriesService> _categoryServiceMock;

        public PostControllerTest()
        {
            _dbContextMock = new Mock<BlogDbContext>();
            _postServiceMock = new Mock<IPostsService>();
            _categoryServiceMock = new Mock<ICategoriesService>();

            var posts = new List<Post>
            {
                new Post { PostId = 1, Title = "Test Post 1" },
                new Post { PostId = 2, Title = "Test Post 2" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _dbContextMock.Setup(db => db.Posts).Returns(mockSet.Object);
        }

        [Fact]
        public async Task PostController_Index_ReturnsViewResult_WithListOfPosts()
        {
            // Arrange
            var controller = new PostController(_dbContextMock.Object, _postServiceMock.Object, _categoryServiceMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Post>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        // Additional test methods for Create, Edit, DeleteConfirmed would follow a similar pattern,
        [Fact]
        public async Task PostController_Create_ReturnsViewResult()
        {
            // Arrange
            var controller = new PostController(_dbContextMock.Object, _postServiceMock.Object, _categoryServiceMock.Object);

            // Act
            var result = await controller.CreateAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task PostController_Edit_ReturnsViewResult_WithPost()
        {
            // Arrange
            int testPostId = 1;
            var categories = new List<Category> { new Category { CategoryId = 1, Name = "Test Category" } };
            _categoryServiceMock.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(categories);
            var controller = new PostController(_dbContextMock.Object, _postServiceMock.Object, _categoryServiceMock.Object);

            // Act
            var result = await controller.Edit(testPostId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            Assert.Equal(testPostId, model.PostId);
            Assert.Equal(categories, viewResult.ViewData["Categories"]);
        }

        [Fact]
        public async Task PostController_DeleteConfirmed_ReturnsRedirectToIndex_WhenPostExists()
        {
            // Arrange
            int testPostId = 1;
            _postServiceMock.Setup(service => service.ExistsById(testPostId)).ReturnsAsync(true);
            _postServiceMock.Setup(service => service.GetPostByIdAsync(testPostId)).ReturnsAsync(new Post { PostId = testPostId });
            _postServiceMock.Setup(service => service.DeletePostByIdAsync(testPostId)).Returns(Task.CompletedTask);
            var controller = new PostController(_dbContextMock.Object, _postServiceMock.Object, _categoryServiceMock.Object);

            // Act
            var result = await controller.DeleteConfirmed(testPostId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _postServiceMock.Verify(service => service.DeletePostByIdAsync(testPostId), Times.Once);
        }
    }
}
