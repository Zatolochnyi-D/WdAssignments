using MyndMapper.Models;
using MyndMapper.Objects;

namespace MyndMapper;

public class Program
{
    public static void Main(string[] args)
    {
        UserAccount.CreateUser(new()
        {
            Name = "Athos",
            Email = "firstOne@nonexistentmail.com",
            Password = "123456",
        });
        UserAccount.CreateUser(new()
        {
            Name = "Porthos",
            Email = "secondOne@nonexistentmail.com",
            Password = "654321",
        });
        UserAccount.CreateUser(new()
        {
            Name = "Aramis",
            Email = "thirdOne@nonexistentmail.com",
            Password = "123321",
        });
        CanvasModel model = new()
        {
            Name = "NewCanvas",
            CreationDate = DateTime.Now,
        };
        model.SetOwnerId(0);
        Canvas.CreateCanvas(model);
        UserAccount.GetUserById(0).CreatedCanvases.Add(0);

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
