namespace UnitTestDemo.Entity;

public class User(
    string name,
    string password,
    string email,
    string phoneNumber,
    int age,
    string address,
    string role,
    DateTime createdAt,
    DateTime updatedAt)
{
    public Guid Id { get; } = Guid.NewGuid();
    private string Name { get; set; } = name;
    private string Password { get; set; } = password;
    private string Email { get; set; } = email;
    private string PhoneNumber { get; set; } = phoneNumber;
    private int Age { get; set; } = age;
    private string Address { get; set; } = address;
    private string Role { get; set; } = role;
    private DateTime CreatedAt { get; set; } = createdAt;
    private DateTime UpdatedAt { get; set; } = updatedAt;
    
    public Guid GetId() => Id;
    public string GetName() => Name;
    public string GetPassword() => Password;
    public string GetEmail() => Email;
    public string GetPhoneNumber() => PhoneNumber;
    public int GetAge() => Age;
    public string GetAddress() => Address;
    public string GetRole() => Role;
    public DateTime GetCreatedAt() => CreatedAt;
    public DateTime GetUpdatedAt() => UpdatedAt;
    
    public void SetName(string name) => Name = name;
    public void SetPassword(string password) => Password = password;
    public void SetEmail(string email) => Email = email;
    public void SetPhoneNumber(string phoneNumber) => PhoneNumber = phoneNumber;
    public void SetAge(int age) => Age = age;
    public void SetAddress(string address) => Address = address;
    public void SetRole(string role) => Role = role;
    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;
}