using Microsoft.AspNetCore.Mvc;
using UnitTestDemo.Db;
using UnitTestDemo.Dto;
using UnitTestDemo.Entity;

namespace UnitTestDemo.Controllers;

public class AuthControllers(UserDbContext userDbContext) : ControllerBase
{
    [Route("/auth/register")]
    [HttpPost]
    public bool Register([FromBody] Register register)
    {

        if (string.IsNullOrWhiteSpace(register.Name) ||
            string.IsNullOrWhiteSpace(register.Password) ||
            string.IsNullOrWhiteSpace(register.Email) ||
            string.IsNullOrWhiteSpace(register.PhoneNumber) ||
            register.Age <= 0 ||
            string.IsNullOrWhiteSpace(register.Address) ||
            string.IsNullOrWhiteSpace(register.Role))
        {
            return false;
        }

        if(userDbContext.Users.Any(u => u.GetEmail() == register.Email))
            return false;
        userDbContext.Users.Add(new User(register.Name, register.Password, register.Email, register.PhoneNumber,
            register.Age, register.Address, register.Role, DateTime.Now, DateTime.Now));
        return true ;
    }

    [Route("/auth/login")]
    [HttpPost]
    public string LogIn([FromBody] LogIn logIn)
    {
        var user = userDbContext.Users.FirstOrDefault(u =>
            u.GetEmail() == logIn.Email && u.GetPassword() == logIn.Password);
        return user != null ? "Login success" : "Login failed";
    }
}