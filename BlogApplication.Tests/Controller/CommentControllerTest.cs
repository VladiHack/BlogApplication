using BlogApplication.Controllers;
using BlogApplication.Models;
using BlogApplication.Services.Comments;
using BlogApplication.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApplication.Tests.Controller
{
    public class CommentControllerTest
    {
        private readonly Mock<BlogDbContext> _dbContextMock;
        private readonly Mock<ICommentsService> _commentsServiceMock;
        private readonly Mock<IPostsService> _postsServiceMock;

        public CommentControllerTest()
        {
            _dbContextMock = new Mock<BlogDbContext>();
            _commentsServiceMock = new Mock<ICommentsService>();
            _postsServiceMock = new Mock<IPostsService>();

            var comments = new List<Comment>
            {
                new Comment { CommentId = 1, Content = "Test Comment 1", PostId = 1 },
                new Comment { CommentId = 2, Content = "Test Comment 2", PostId = 2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Comment>>();
            mockSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(comments.Provider);
            mockSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(comments.Expression);
            mockSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(comments.ElementType);
            mockSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(comments.GetEnumerator());

            _dbContextMock.Setup(db => db.Comments).Returns(mockSet.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfComments()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            int postId = 1;

            // Act
            var result = await controller.Index(postId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Comment>>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }

        [Fact]
        public async Task Create_ReturnsViewResult()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            int postId = 1;

            // Act
            var result = await controller.Create(postId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(postId, viewResult.ViewData["PostId"]);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenValid()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            var comment = new Comment { Content = "Test Comment", PostId = 1 };
            _commentsServiceMock.Setup(s => s.GetCommentsAsync()).ReturnsAsync(new List<Comment>());
            _commentsServiceMock.Setup(s => s.CreateCommentAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            // Act
            var result = await controller.Create(comment.PostId, comment);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithComment()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            int commentId = 1;
            var comment = new Comment { CommentId = commentId, Content = "Test Comment", PostId = 1 };
            _dbContextMock.Setup(db => db.Comments.FindAsync(commentId)).ReturnsAsync(comment);

            // Act
            var result = await controller.Edit(commentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Comment>(viewResult.ViewData.Model);
            Assert.Equal(commentId, model.CommentId);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithComment()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            int commentId = 1;
            var comment = new Comment { CommentId = commentId, Content = "Test Comment", PostId = 1 };
            _dbContextMock.Setup(db => db.Comments.FindAsync(commentId)).ReturnsAsync(comment);

            // Act
            var result = await controller.Delete(commentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Comment>(viewResult.ViewData.Model);
            Assert.Equal(commentId, model.CommentId);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            int commentId = 1;
            _commentsServiceMock.Setup(s => s.GetCommentByIdAsync(commentId)).ReturnsAsync(new Comment { CommentId = commentId });
            _commentsServiceMock.Setup(s => s.DeleteCommentByIdAsync(commentId)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteConfirmed(commentId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithComment()
        {
            // Arrange
            var controller = new CommentController(_dbContextMock.Object, _commentsServiceMock.Object, _postsServiceMock.Object);
            int commentId = 1;
            var comment = new Comment { CommentId = commentId, Content = "Test Comment", PostId = 1 };
            _dbContextMock.Setup(db => db.Comments.FindAsync(commentId)).ReturnsAsync(comment);

            // Act
            var result = await controller.Details(commentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Comment>(viewResult.ViewData.Model);
            Assert.Equal(commentId, model.CommentId);
        }
    }
}
