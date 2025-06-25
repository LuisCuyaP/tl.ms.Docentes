using Docentes.Domain.Abstractions;

namespace Docentes.Domain.Test;

public class ResultTest
{
    [Fact]
    public void Result_Success_ShouldReturnSuccess()
    {
        // Arrange
        var result = Result.Success();
        // Act
        var isSuccess = result.IsSuccess;
        // Assert
        Assert.True(isSuccess);
    }

    [Fact]
    public void Result_Failure_ShouldReturnFailure()
    {
        // Arrange
        var errorMessage = new Error("Test", "An error occurred");
        var result = Result.Failure(errorMessage);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errorMessage, result.Error);
    }

    [Fact]
    public void SuccessTValue_ShouldReturnSuccessValue()
    {
        // Arrange
        var value = "Test Value";
        var result = Result.Success(value);
        // Act
        var isSuccess = result.IsSuccess;
        var succesValue = result.Value;
        // Assert
        Assert.True(isSuccess);
        Assert.Equal(value, succesValue);
    }

    
}