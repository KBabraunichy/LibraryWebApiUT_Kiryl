
namespace LibraryWebApi.Tests
{
    public class UserControllerTest
    {
        private readonly Mock<ILoggerException> _mockLogger;
        private readonly Mock<IAuthRepository<User, UserLogin>> _mockRepo;
        private readonly UserController _controller;
        private readonly User _testUser;

        public UserControllerTest()
        {
            _mockLogger = new Mock<ILoggerException>();
            _mockRepo = new Mock<IAuthRepository<User, UserLogin>>();
            _controller = new UserController(_mockRepo.Object, _mockLogger.Object);

            _testUser = new User()
            {
                Username = "testUser",
                Password = "testPassword",
                Email = "sample@sample.com",
                Role = "testRole"
            };
        }

        [Fact]
        public void RegisterUser_NullPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange

            // Act
            var badResponse = _controller.RegistrateUser(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result.Result);
        }
       
        [Fact]
        
        public void RegistrateUser_ValidUserPassed_ReturnsCreatedResponse()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Registration(_testUser)).Returns(Task.FromResult(_testUser));

            // Act
            var createdResponse = _controller.RegistrateUser(_testUser);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);
        }

        [Fact]
        public void RegistrateUser_ExistedUserNamePassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.RegistrationCheck(_testUser)).Returns(true);

            // Act
            var badResponse = _controller.RegistrateUser(_testUser);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result.Result);
        }

        [Fact]
        public void RegistrateUser_ValidUserPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Registration(_testUser)).Returns(Task.FromResult(_testUser));

            // Act
            var createdResponse = _controller.RegistrateUser(_testUser).Result.Result as CreatedAtActionResult;
            var item = createdResponse.Value as User;

            // Assert
            Assert.IsType<User>(item);
            Assert.Equal("testUser", item.Username);
        } 
    }
}