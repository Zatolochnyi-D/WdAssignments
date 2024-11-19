using System.ComponentModel.DataAnnotations;
using MyndMapper.Objects;

namespace MyndMapper.Models;

public class UserModel
{
    public static UserModel CreateFromUser(UserAccount user)
    {
        UserModel userModel = new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            CreatedCanvases = user.CreatedCanvases,
        };
        return userModel;
    }

    public UserModel()
    {
        Id = null;
        CreatedCanvases = [];
    }

    public int? Id { get; private set; }

    [MinLength(2)]
    public required string Name { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(5)]
    public required string Password { get; set; }

    public List<int> CreatedCanvases { get; private set; }
}