using Moq;
using UnitTestDemo.Controllers;
using UnitTestDemo.Db;
using UnitTestDemo.Dto;
using UnitTestDemo.Entity;

namespace UnitTest;

public class AuthControllers
{
    [Fact]
    public void Login_With_Correct_Credentials_Returns_Success()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var user = new User("TestUser", "Password123", "test@example.com", "1234567890", 30, "123 Test St", "User",
            DateTime.Now, DateTime.Now);
        var users = new List<User> { user };

        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "test@example.com", Password = "Password123" };

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login success", result);
    }

    [Fact]
    public void Login_With_NonExistent_Email_Returns_Failed()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var users = new List<User>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "nonexistent@example.com", Password = "Password123" };

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login failed", result);
    }

    [Fact]
    public void Register_With_Valid_Data_Returns_True()
    {
        // Arrange
        var users = new List<User>();
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var register = new Register
        {
            Name = "Valid User",
            Password = "validpassword",
            Email = "valid@example.com",
            PhoneNumber = "1234567890",
            Age = 25,
            Address = "123 Valid St",
            Role = "User"
        };

        // Act
        var result = controller.Register(register);

        // Assert
        Assert.True(result);
        Assert.Single(users);
    }

    [Fact]
    public void Register_With_Duplicate_Email_Returns_False()
    {
        // Arrange
        var existingUser = new User("Existing User", "password", "existing@example.com", "1234567890", 30, "123 Existing St", "User",
            DateTime.Now, DateTime.Now);
        var users = new List<User> { existingUser };
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var register = new Register
        {
            Name = "New User",
            Password = "newpassword",
            Email = "existing@example.com", // Same email as existing user
            PhoneNumber = "0987654321",
            Age = 25,
            Address = "456 New St",
            Role = "User"
        };

        // Act
        var result = controller.Register(register);

        // Assert
        Assert.False(result);
        Assert.Single(users); // No new user should be added
    }

    [Fact]
    public void Register_With_Zero_Age_Returns_False()
    {
        // Arrange
        var users = new List<User>();
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);

        var register = new Register
        {
            Name = "Test User",
            Password = "password123",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Age = 0, // Testing with zero age
            Address = "123 Test St",
            Role = "User"
        };

        // Act
        var result = controller.Register(register);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Register_With_Negative_Age_Returns_False()
    {
        // Arrange
        var users = new List<User>();
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);

        var register = new Register
        {
            Name = "Test User",
            Password = "password123",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Age = -5, // Testing with negative age
            Address = "123 Test St",
            Role = "User"
        };

        // Act
        var result = controller.Register(register);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("username", "password", "email@test.com", null, 25, "Address", "User")]
    [InlineData("username", "password", "email@test.com", "", 25, "Address", "User")]
    [InlineData("username", "password", "email@test.com", "123456789", 25, null, "User")]
    [InlineData("username", "password", "email@test.com", "123456789", 25, "", "User")]
    [InlineData("username", "password", "email@test.com", "123456789", 25, "Address", null)]
    [InlineData("username", "password", "email@test.com", "123456789", 25, "Address", "")]
    public void Register_With_Other_Missing_Fields_Returns_False(string name, string password, string email,
        string phoneNumber, int age, string address, string role)
    {
        // Arrange
        var users = new List<User>();
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);

        var register = new Register
        {
            Name = name,
            Password = password,
            Email = email,
            PhoneNumber = phoneNumber,
            Age = age,
            Address = address,
            Role = role
        };

        // Act
        var result = controller.Register(register);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Login_With_Case_Sensitive_Password_Fails()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var user = new User("TestUser", "Password123", "test@example.com", "1234567890", 30, "123 Test St", "User",
            DateTime.Now, DateTime.Now);
        var users = new List<User> { user };

        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "test@example.com", Password = "password123" }; // Different case

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login failed", result);
    }

    [Fact]
    public void Register_Adds_User_With_Correct_Properties()
    {
        // Arrange
        var users = new List<User>();
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var registerDto = new Register
        {
            Name = "New Test User",
            Password = "securepassword",
            Email = "newtest@example.com",
            PhoneNumber = "9876543210",
            Age = 28,
            Address = "456 New St",
            Role = "Admin"
        };

        // Act
        var result = controller.Register(registerDto);

        // Assert
        Assert.True(result);
        //Assert.Single(users);
        var addedUser = users[0];
        Assert.Equal("New Test User", addedUser.GetName());
        Assert.Equal("securepassword", addedUser.GetPassword());
        Assert.Equal("newtest@example.com", addedUser.GetEmail());
        Assert.Equal("9876543210", addedUser.GetPhoneNumber());
        Assert.Equal(28, addedUser.GetAge());
        Assert.Equal("456 New St", addedUser.GetAddress());
        Assert.Equal("Admin", addedUser.GetRole());
    }

    [Fact]
    public void Login_With_Whitespace_In_Email_Or_Password()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var user = new User("TestUser", "Password123", "test@example.com", "1234567890", 30, "123 Test St", "User",
            DateTime.Now, DateTime.Now);
        var users = new List<User> { user };

        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = " test@example.com ", Password = " Password123 " }; // With whitespace

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login failed", result);
    }
}

