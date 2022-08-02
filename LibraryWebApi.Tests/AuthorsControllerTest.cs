
namespace LibraryWebApi.Tests
{
    public class AuthorsControllerTest
    {
        private readonly Mock<IRepository<Author>> _mockRepo;
        private readonly Mock<ILoggerException> _mockLogger;
        private readonly AuthorsController _controller;

        private readonly int _testId;

        private readonly Author _testAuthor;

        public AuthorsControllerTest()
        {
            _mockRepo = new Mock<IRepository<Author>>();
            _mockLogger = new Mock<ILoggerException>();
            _controller = new AuthorsController(_mockRepo.Object, _mockLogger.Object);

            _testId = 1;
            _testAuthor = new Author()
            {
                Id = 1,
                FirstName = "testFirstName",
                LastName = "testLastName",
                SurName = "testSurName",
                BirthDate = DateTime.MinValue
            };
        }

        //-----------GetAuthors

        [Fact]
        public void GetAuthors_WhenCalled_ReturnsOkObjectResult()
        {
            // Arrange

            // Act
            var okResponse = _controller.GetAuthors();

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        //-----------GetAuthor

        [Fact]
        public void GetAuthor_WhenCalled_ReturnsOkObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testAuthor));

            // Act
            var okResponse = _controller.GetAuthor(_testId);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result.Result);
        }

        [Fact]
        public void GetAuthor_WhenCalled_ReturnsNotFoundResult()
        {
            // Arrange
            int testId = 0;

            // Act
            var notFoundResponse = _controller.GetAuthor(testId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResponse.Result.Result);
        }

        //-----------UpdateAuthor

        [Fact]
        public void UpdateAuthor_IdMismatch_ReturnsBadRequestObjectResult()
        {
            // Arrange
            int id = 10;

            // Act
            var badResponse = _controller.UpdateAuthor(id, _testAuthor);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result.Result);
        }

        [Fact]

        public void UpdateAuthor_ValidAuthorAndIdPassed_ReturnsOkObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testAuthor));

            // Act
            var okResponse = _controller.UpdateAuthor(_testId, _testAuthor);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result.Result);
        }

        [Fact]
        public void UpdateAuthor_NoAuthorWithProvidedId_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(null as Author));

            // Act
            var notFoundResponse = _controller.UpdateAuthor(_testId, _testAuthor);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse.Result.Result);

        }



        //-----------CreateAuthor

        [Fact]
        public void CreateAuthor_InvalidAuthorPassed_ReturnsBadRequestResult()
        {
            // Arrange

            // Act
            var badResponse = _controller.CreateAuthor(null);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse.Result.Result);
        }


        [Fact]
        public void CreateAuthor_ValidAuthorPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Create(_testAuthor)).Returns(Task.FromResult(_testAuthor));

            // Act
            var createdResponse = _controller.CreateAuthor(_testAuthor).Result.Result as CreatedAtActionResult;
            var item = createdResponse.Value as Author;

            // Assert
            Assert.IsType<Author>(item);
            Assert.Equal("testFirstName", item.FirstName);
        }

        //-----------DeleteAuthor

        [Fact]
        public void DeleteAuthor_ValidIdPassed_ReturnsNoContentResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(_testAuthor));

            // Act
            var noContentResponse = _controller.DeleteAuthor(_testId);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse.Result.Result);
        }

        [Fact]
        public void DeleteAuthor_NoAuthorWithProvidedId_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetObject(_testId)).Returns(Task.FromResult(null as Author));

            // Act
            var notFoundResponse = _controller.DeleteAuthor(_testId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse.Result.Result);
        }

    }
}