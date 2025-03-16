using UnitTestDemo.Dto;
using UnitTestDemo.Entity;

namespace UnitTestDemo.MapConfigure;

public interface IMapper<in TSrc, out T>
{
    public T Configure(TSrc source);
}

public class UserMapper : IMapper<User, UserDto>
{
    public UserDto Configure(User source)
    {
        return new UserDto
        {
            Id = source.Id,
            Name = source.GetName(),
            Email = source.GetEmail(),
            PhoneNumber = source.GetPhoneNumber(),
            Age = source.GetAge(),
            Address = source.GetAddress()
        };
    }

    public User ReverseConfigure(UserDto dest)
    {
        return new User(dest.Name, "dfsd", dest.Email, dest.PhoneNumber, dest.Age, dest.Address, "asfadsf", DateTime.Now, 
            DateTime.Now);
    }
}