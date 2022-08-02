

using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibraryWebApi.Tests
{
    public class LoginControllerTest
    {
        private readonly Mock<IAuthRepository<User, UserLogin>> _mockRepo;
        private readonly LoginController _controller;
        private readonly UserLogin _testUserLogin;
        private readonly User _testUser;

        public LoginControllerTest()
        {
            _mockRepo = new Mock<IAuthRepository<User, UserLogin>>();
            _controller = new LoginController(_mockRepo.Object);

            _testUserLogin = new UserLogin()
            {
                Username = "testUser",
                Password = "testPassword"
            };

            _testUser = new User()
            {
                Username = "testUser",
                Password = "testPassword",
                Email = "sample@sample.com",
                Role = "testRole"
            };

        }

        //-----------Login

        [Fact]
        public void Login_NullPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange

            // Act
            var badResponse = _controller.Login(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
        
        [Fact]
        public void Login_IncorrectUserLoginPassed_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Authenticate(_testUserLogin)).Returns(null as User);

            // Act
            var notFoundResponse = _controller.Login(_testUserLogin);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse);
        }

        [Fact]
        public void Login_ValidUserPassed_ReturnsOkObjectResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Authenticate(_testUserLogin)).Returns(_testUser);
            _mockRepo.Setup(repo => repo.Generate(_testUser)).Returns("mock-access-token");

            // Act
            var okResponse = _controller.Login(_testUserLogin);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse);
        }

        // GetRole

        [Fact]
        public void GetRole_InvalidTokenPassed_ReturnsNotFoundObjectResult()
        {
            // Arrange

            // Act
            var notFoundResponse = _controller.GetRole();

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResponse);
        }

        [Fact]
        public void GetRole_ValidTokenPassed_ReturnsOkObjectResult()
        {
            // Arrange
            var userClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "testRole")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaimsPrincipal }
            };

            // Act
            var okResponse = _controller.GetRole();

            // Assert
            Assert.IsType<OkObjectResult>(okResponse);
        }
    }
}