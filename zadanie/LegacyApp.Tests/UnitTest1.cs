using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace LegacyApp.Tests;

public class UnitTest1
{
    [Fact]
    public void AddUser_ReturnsFalseWhenFirstNameIsEmpty()
    {
        //Arrange
        var userSerivce = new UserService();

        //Act
        
        var result = userSerivce.AddUser(
            null, "Kowalskki",
            "kowalski@gmail.com",
            DateTime.Parse("2000-01-01"),
            1
        );
        
        //Assert
        Assert.False(result);
        
    }
    
    [Fact]
    public void AddUser_ThrowsExceptionWhenClientDoesNotExist()
    {
        //Arrange
        var userSerivce = new UserService();

        //Act
        
        
        Action action = () =>
        {
            var result = userSerivce.AddUser(
                "Jan", "Kowalskki",
                "kowalski@gmail.com",
                DateTime.Parse("2000-01-01"),
                100
            );
        };


        //Assert
        Assert.Throws<ArgumentException>(action);
        
    }
    
    
    
    
}