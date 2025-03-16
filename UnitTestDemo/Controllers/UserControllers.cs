using Microsoft.AspNetCore.Mvc;
using UnitTestDemo.Db;
using UnitTestDemo.Dto;
using UnitTestDemo.Entity;
using UnitTestDemo.MapConfigure;

namespace UnitTestDemo.Controllers;

public class UserControllers(UserDbContext userDbContext, IMapper<User, UserDto> mapper) : ControllerBase
{
    [Route("/user")]
    [HttpGet]
    public List<UserDto> GetUser()
    {
        var listUser = userDbContext.Users;
        var result = new List<UserDto>();
        foreach (var user in listUser)
        {
            result.Add(mapper.Configure(user));
        }
       
        return result;
    }
}