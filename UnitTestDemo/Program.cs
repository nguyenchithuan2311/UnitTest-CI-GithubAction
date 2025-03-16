using UnitTestDemo.Db;
using UnitTestDemo.Dto;
using UnitTestDemo.Entity;
using UnitTestDemo.MapConfigure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<UserDbContext>();
builder.Services.AddSingleton<IMapper<User, UserDto>, UserMapper>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();



app.Run();