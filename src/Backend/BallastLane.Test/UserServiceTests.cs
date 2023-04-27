using BallastLane.Domain.Entities;
using BallastLane.Domain.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace BallastLane.Test;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly IValidator<User> _userValidator;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _userValidator = new UserValidator();
        _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object, _userValidator);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldCreateUser_WhenUserModelIsValid()
    {
        // Arrange
        var userModel = new RegisterUserModel
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        _userRepositoryMock
            .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(new User
            {
                Id = 1,
                Username = userModel.Username,
                Email = userModel.Email,
                PasswordHash = new byte[64],
                PasswordSalt = new byte[128]
            });

        // Act
        var result = await _userService.RegisterUserAsync(userModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userModel.Username, result.Username);
        Assert.Equal(userModel.Email, result.Email);
        _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync(userModel.Username), Times.Once);
        _userRepositoryMock.Verify(x => x.CreateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var userModel = new RegisterUserModel
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User { Id = 1 });

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterUserAsync(userModel));
        _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync(userModel.Username), Times.Once);
        _userRepositoryMock.Verify(x => x.CreateUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowValidationException_WhenUserModelIsInvalid()
    {
        // Arrange
        var userModel = new RegisterUserModel
        {
            Username = "",
            Email = "",
            Password = ""
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(async () => await _userService.RegisterUserAsync(userModel));
        Assert.Contains("Username is required.", ex.Message);
        Assert.Contains("Email address is required.", ex.Message);
    }

}