using UnitTestDemo.Entity;

namespace UnitTestDemo.Db;

public class UserDbContext
{
    public virtual List<User> Users { get; set; } = [];

    public UserDbContext()
    {
        Users.Add(new User("admin", "admin", "email@gmail.com", "1234567890", 20, "address", "admin", DateTime.Now,
            DateTime.Now));
        Users.Add(new User("user", "user", "a", "1234567890", 20, "address", "user", DateTime.Now, DateTime.Now));
    }
}