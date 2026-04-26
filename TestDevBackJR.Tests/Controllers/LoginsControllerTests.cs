using Moq;
using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Application.Interfaces;
using TestDevBackJR.Controllers;
using TestDevBackJR.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TestDevBackJR.Tests.Controllers;

public class LoginsControllerTests
{
    private readonly Mock<ILoginService> _mockLoginService;
    private readonly LoginsController _controller;

    public LoginsControllerTests()
    {
        _mockLoginService = new Mock<ILoginService>();
        _controller = new LoginsController(_mockLoginService.Object);
    }

    #region GET Tests

    [Fact]
    public async Task Get_ReturnsOkResult_WithLoginsList()
    {
        // Arrange
        var logins = new List<Login>
        {
            new Login { Id = 1, UserId = 1, Extension = 100, MovementType = 1, Date = DateTime.Now },
            new Login { Id = 2, UserId = 2, Extension = 101, MovementType = 2, Date = DateTime.Now }
        };

        _mockLoginService.Setup(s => s.GetAll())
            .ReturnsAsync(logins);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedLogins = Assert.IsAssignableFrom<IEnumerable<Login>>(okResult.Value);
        Assert.Equal(2, returnedLogins.Count());
    }

    [Fact]
    public async Task Get_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var errorMessage = "Database connection error";
        _mockLoginService.Setup(s => s.GetAll())
            .ThrowsAsync(new Exception(errorMessage));

        // Act
        var result = await _controller.Get();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);

        var responseValue = statusCodeResult.Value;
        var messageProperty = responseValue?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal(errorMessage, messageProperty.GetValue(responseValue));
    }

    #endregion

    #region POST Tests

    [Fact]
    public async Task Post_ReturnsCreatedAtAction_WithValidLoginDto()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UserId = 1,
            Extension = 100,
            MovementType = 1,
            Date = DateTime.Now
        };

        var createdLogin = new Login
        {
            Id = 1,
            UserId = loginDto.UserId,
            Extension = loginDto.Extension,
            MovementType = loginDto.MovementType,
            Date = loginDto.Date
        };

        _mockLoginService.Setup(s => s.Create(It.IsAny<LoginDto>()))
            .ReturnsAsync(createdLogin);

        // Act
        var result = await _controller.Post(loginDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(_controller.Get), createdResult.ActionName);
        Assert.Equal(201, createdResult.StatusCode);

        var returnedLogin = Assert.IsType<Login>(createdResult.Value);
        Assert.Equal(1, returnedLogin.Id);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidOperationException()
    {
        // Arrange
        var loginDto = new LoginDto { UserId = 0, Extension = 100, MovementType = 1, Date = DateTime.Now };
        var errorMessage = "El usuario especificado no existe";

        _mockLoginService.Setup(s => s.Create(It.IsAny<LoginDto>()))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

        // Act
        var result = await _controller.Post(loginDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var responseValue = badRequestResult.Value;
        var messageProperty = responseValue?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal(errorMessage, messageProperty.GetValue(responseValue));
    }

    #endregion

    #region PUT Tests

    [Fact]
    public async Task Put_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var id = 1;
        var loginDto = new LoginDto { UserId = 1, Extension = 100, MovementType = 1, Date = DateTime.Now };

        _mockLoginService.Setup(s => s.Update(id, It.IsAny<LoginDto>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Put(id, loginDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenLoginDoesNotExist()
    {
        // Arrange
        var id = 999;
        var loginDto = new LoginDto { UserId = 1, Extension = 100, MovementType = 1, Date = DateTime.Now };

        _mockLoginService.Setup(s => s.Update(id, It.IsAny<LoginDto>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Put(id, loginDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var responseValue = notFoundResult.Value;
        var messageProperty = responseValue?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal($"Login con ID {id} no encontrado", messageProperty.GetValue(responseValue));
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var id = 1;
        var loginDto = new LoginDto { UserId = 0, Extension = 100, MovementType = 1, Date = DateTime.Now };
        var errorMessage = "El usuario especificado no existe";

        _mockLoginService.Setup(s => s.Update(id, It.IsAny<LoginDto>()))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

        // Act
        var result = await _controller.Put(id, loginDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var responseValue = badRequestResult.Value;
        var messageProperty = responseValue?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal(errorMessage, messageProperty.GetValue(responseValue));
    }

    #endregion

    #region DELETE Tests

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        var id = 1;
        _mockLoginService.Setup(s => s.Delete(id))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenLoginDoesNotExist()
    {
        // Arrange
        var id = 999;
        _mockLoginService.Setup(s => s.Delete(id))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var responseValue = notFoundResult.Value;
        var messageProperty = responseValue?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal($"Login con ID {id} no encontrado", messageProperty.GetValue(responseValue));
    }

    [Fact]
    public async Task Delete_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var id = 1;
        var errorMessage = "Database error";
        _mockLoginService.Setup(s => s.Delete(id))
            .ThrowsAsync(new Exception(errorMessage));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);

        var responseValue = statusCodeResult.Value;
        var messageProperty = responseValue?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal(errorMessage, messageProperty.GetValue(responseValue));
    }

    #endregion
}