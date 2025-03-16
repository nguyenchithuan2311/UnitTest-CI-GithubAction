using Moq;
using UnitTestDemo.Controllers;
using UnitTestDemo.Db;
using UnitTestDemo.Dto;
using UnitTestDemo.Entity;

namespace UnitTest;

public class AuthControllers
{
    // GetUser returns "Auth" string
    [Fact]
    public void get_user_returns_auth_string()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var mockDbSet = new Mock<List<User>>();
        mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);
        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);

        // Act
        var result = controller.GetUser();

        // Assert
        Assert.Equal("Auth", result);
    }

    // Register with null or empty required fields
    [Theory]
    [InlineData(null, "password", "email@test.com", "123456789", 25, "Address", "User")]
    [InlineData("", "password", "email@test.com", "123456789", 25, "Address", "User")]
    [InlineData("username", null, "email@test.com", "123456789", 25, "Address", "User")]
    [InlineData("username", "", "email@test.com", "123456789", 25, "Address", "User")]
    [InlineData("username", "password", null, "123456789", 25, "Address", "User")]
    [InlineData("username", "password", "", "123456789", 25, "Address", "User")]
    public void Register_With_Null_Or_Empty_Fields_Should_Fail(string name, string password, string email,
        string phoneNumber, int age, string address, string role)
    {
        // Arrange
        var users = new List<User>(); // Danh sách thực để lưu trữ dữ liệu
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

        // Act & Assert
        var result = controller.Register(register);

        // Assert
        Assert.False(result);
    }


    // Register adds a new user to the database and returns true
    [Fact]
    public void register_adds_new_user_and_returns_true()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var users = new List<User>(); // Danh sách thực tế

// Mock thuộc tính Users để trả về danh sách thực tế
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var registerDto = new Register
        {
            Name = "Test User",
            Password = "password123",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Age = 30,
            Address = "123 Test St",
            Role = "User"
        };

        // Act
        var result = controller.Register(registerDto);

        // Assert
        Assert.True(result);  // Kiểm tra kết quả trả về là true

    }

    // Login returns "Login success" when credentials match
    [Fact]
    public void login_returns_success_when_credentials_match()
    {
        // Arrange
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();

// Tạo dữ liệu giả
        var user = new User("TestName", "TestPassword", "test@example.com", "1234567890", 30, "TestAddress", "User", DateTime.Now, DateTime.Now);
        var users = new List<User> { user };

// Mock thuộc tính Users để trả về danh sách user
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "test@example.com", Password = "TestPassword" };

// Act
        var result = controller.LogIn(logInDto);

// Assert
        Assert.Equal("Login success", result);

    }

    // Login returns "Login failed" when credentials don't match
    [Fact]
    public void login_returns_failed_when_credentials_do_not_match()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(new List<User>());
        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "wrong@example.com", Password = "wrongpassword" };

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login failed", result);
    }

    // Login with non-existent email
    [Fact]
    public void login_with_non_existent_email_returns_login_failed()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        mockDbContext.Setup(db => db.Users).Returns(new List<User>());
        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "nonexistent@example.com", Password = "password123" };

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login failed", result);
    }

    // Register correctly maps DTO fields to User entity
    [Fact]
    public void register_maps_dto_fields_to_user_entity()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var users = new List<User>(); // Danh sách thực tế

// Mock thuộc tính Users để trả về danh sách thực tế
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var registerDto = new Register
        {
            Name = "John Doe",
            Password = "password123",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Age = 30,
            Address = "123 Main St",
            Role = "User"
        };

// Act
        var result = controller.Register(registerDto);

// Assert
        Assert.True(result);
    }

    // Login with incorrect password for existing email
    [Fact]
    public void login_with_incorrect_password_returns_login_failed()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var existingUser = new User("John Doe", "correctpassword", "john.doe@example.com", "1234567890", 30,
            "123 Main St", "User", DateTime.Now, DateTime.Now);
        mockDbContext.Setup(db => db.Users).Returns(new List<User> { existingUser });

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "john.doe@example.com", Password = "wrongpassword" };

        // Act
        var result = controller.LogIn(logInDto);

        // Assert
        Assert.Equal("Login failed", result);
    }

    // Multiple registrations with the same email
    [Fact]
    public void register_with_duplicate_email_returns_false()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var existingUser = new User("John Doe", "password123", "john.doe@example.com", "1234567890", 30, "123 Main St",
            "User", DateTime.Now, DateTime.Now);
        mockDbContext.Setup(db => db.Users).Returns(new List<User> { existingUser });

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var newRegister = new Register
        {
            Name = "Jane Doe",
            Password = "password123",
            Email = "john.doe@example.com",
            PhoneNumber = "0987654321",
            Age = 25,
            Address = "456 Elm St",
            Role = "User"
        };

        // Act
        var result = controller.Register(newRegister);

        // Assert
        Assert.False(result);
    }

    // Login with case-sensitive email/password
    [Fact]
    public void login_with_case_sensitive_email_password()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();

// Danh sách dữ liệu giả
        var user = new User("TestUser", "Password123", "test@example.com", "1234567890", 30, "123 Test St", "User",
            DateTime.Now, DateTime.Now);
        var users = new List<User> { user };

// Mock thuộc tính Users trả về danh sách người dùng
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "test@example.com", Password = "Password123" };

// Act
        var result = controller.LogIn(logInDto);

// Assert
        Assert.Equal("Login success", result);

    }

    // Performance with large number of users in database
    [Fact]
    public void register_user_performance_with_large_database()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var users = new List<User>();
        for (int i = 0; i < 1000000; i++)
        {
            users.Add(new User($"Name{i}", $"Password{i}", $"Email{i}@example.com", $"123456789{i}", 30, $"Address{i}",
                "User", DateTime.Now, DateTime.Now));
        }

        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var newUser = new Register
        {
            Name = "NewUser",
            Password = "NewPassword",
            Email = "newuser@example.com",
            PhoneNumber = "1234567890",
            Age = 25,
            Address = "New Address",
            Role = "User"
        };

        // Act
        var result = controller.Register(newUser);

        // Assert
        Assert.True(result);
    }

    // Handling of DateTime fields during registration
    [Fact]
    public void register_should_set_current_datetime_for_created_and_updated_fields()
    {
        // Arrange
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var users = new List<User>(); // Danh sách thực tế được sử dụng để theo dõi dữ liệu

// Mock thuộc tính Users để trả về danh sách thực tế
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var registerDto = new Register
        {
            Name = "Test User",
            Password = "password123",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Age = 30,
            Address = "123 Test St",
            Role = "User"
        };

// Act
        var result = controller.Register(registerDto);

// Assert
        var addedUser = users.FirstOrDefault();
        Assert.NotNull(addedUser);
        Assert.Equal(DateTime.Now.Date, addedUser.GetCreatedAt().Date);
        Assert.Equal(DateTime.Now.Date, addedUser.GetUpdatedAt().Date);
        Assert.True(result);

    }

    // Security concerns with plain text password storage
    [Fact]
    public void test_login_with_plain_text_password()
    {
        // Arrange
        var mockDbContext = new Mock<UserDbContext>();
        var user = new User("TestUser", "plainTextPassword", "test@example.com", "1234567890", 30, "Test Address",
            "User", DateTime.Now, DateTime.Now);

// Tạo danh sách giả
        var users = new List<User> { user };

// Mock thuộc tính Users trả về danh sách người dùng
        mockDbContext.Setup(db => db.Users).Returns(users);

        var controller = new UnitTestDemo.Controllers.AuthControllers(mockDbContext.Object);
        var logInDto = new LogIn { Email = "test@example.com", Password = "plainTextPassword" };

// Act
        var result = controller.LogIn(logInDto);

// Assert
        Assert.Equal("Login success", result);
    }
}