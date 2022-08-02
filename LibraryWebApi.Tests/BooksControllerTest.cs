
namespace LibraryWebApi.Tests
{
    public class BooksControllerTest
    {
        private readonly Mock<IRepository<Book>> _mockBookRepo;
        private readonly Mock<IRepository<Author>> _mockAuthorRepo;
        private readonly Mock<ILoggerException> _mockLogger;
        private readonly BooksController _controller;

        private readonly int _testId;

        private readonly Book _testBook;
        private readonly Author _testAuthor;

        public BooksControllerTest()
        {
            _mockBookRepo = new Mock<IRepository<Book>>();
            _mockAuthorRepo = new Mock<IRepository<Author>>();
            _mockLogger = new Mock<ILoggerException>();
            _controller = new BooksController(_mockBookRepo.Object, _mockAuthorRepo.Object, _mockLogger.Object);

            _testId = 1;
            _testBook = new Book()
            {
                Id = 1,
                Name = "testName",
                Year = int.MinValue,
                AuthorId = 1
            };

            _testAuthor = new Author()
            {
                Id = 1,
                FirstName = "testFirstName",
                LastName = "testLastName",
                SurName = "testSurName",
                BirthDate = DateTime.MinValue
            };

        }

        //-----------GetBooks

        [Fact]
        public void GetBooks_WhenCalled_ReturnsOkObjectResult()
        {
            // Arrange

            // Act
            var okResponse = _controller.GetBooks();

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        //-----------GetBook

        [Fact]
        public void GetBook_WhenCalled_ReturnsOkObjectResult()
        {
            // Arrange
            int testId = 12;
            _mockBookRepo.Setup(repo => repo.GetObject(testId)).Returns(Task.FromResult(_testBook));

            // Act
            var okResponse = _controller.GetBook(testId);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result.Result);
        }

        [Fact]
        public void GetBook_WhenCalled_ReturnsNotFoundResult()
        {
            // Arrange
            int testId = 0;

            // Act
            var notFoundResponse = _controller.GetBook(testId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResponse.Result.Result);
        }

        //-----------UpdateBook

        [Fact]
        public void UpdateBook_IdMismatch_ReturnsBadRequestObjectResult()
        {
            // Arrange
            int id = 10;

            // Act
            var badResponse = _controller.UpdateBook(id, _testBook);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result.Result);
        }

        [Fact]
        public void UpdateBook_NoAuthorWithProvidedAuthorId_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockAuthorRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(null as Author));

            // Act
            var notFoundResponse = _controller.UpdateBook(_testId, _testBook);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse.Result.Result);

        }

        [Fact]
        public void UpdateBook_NoBookWithProvidedId_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockBookRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(null as Book));

            // Act
            var notFoundResponse = _controller.UpdateBook(_testId, _testBook);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse.Result.Result);

        }

        [Fact]
        public void UpdateBook_ValidBookAndIdPassed_ReturnsOkObjectResult()
        {
            // Arrange
            _mockBookRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testBook));
            _mockAuthorRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testAuthor));

            // Act
            var okResponse = _controller.UpdateBook(_testId, _testBook);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result.Result);
        }

        //-----------CreateBook

        [Fact]
        public void CreateBook_InvalidBookPassed_ReturnsBadRequestResult()
        {
            // Arrange

            // Act
            var badResponse = _controller.CreateBook(null);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse.Result.Result);
        }

        [Fact]
        public void CreateBook_NoAuthorWithProvidedAuthorId_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockAuthorRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(null as Author));

            // Act
            var notFoundResponse = _controller.CreateBook(_testBook);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse.Result.Result);

        }

        [Fact]
        public void CreateBook_ValidBookPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            _mockBookRepo.Setup(repo => repo.Create(_testBook)).Returns(Task.FromResult(_testBook));
            _mockAuthorRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testAuthor));

            // Act
            var createdResponse = _controller.CreateBook(_testBook).Result.Result as CreatedAtActionResult;
            var item = createdResponse.Value as Book;

            // Assert
            Assert.IsType<Book>(item);
            Assert.Equal("testName", item.Name);
        }

        //-----------DeleteBook

        [Fact]
        public void DeleteBook_ValidIdPassed_ReturnsNoContentResult()
        {
            // Arrange
            _mockBookRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testBook));

            // Act
            var noContentResponse = _controller.DeleteBook(_testId);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse.Result.Result);
        }

        [Fact]
        public void DeleteBook_NoBookWithProvidedId_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockBookRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(null as Book));

            // Act
            var notFoundResponse = _controller.DeleteBook(_testId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse.Result.Result);
        }

    }
}