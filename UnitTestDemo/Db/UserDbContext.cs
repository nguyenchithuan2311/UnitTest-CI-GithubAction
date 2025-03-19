using UnitTestDemo.Entity;

namespace UnitTestDemo.Db;

public class UserDbContext
{
    public virtual List<User> Users { get; set; } = [];

    public UserDbContext()
    {
    }
}