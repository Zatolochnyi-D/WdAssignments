using MyndMapper.Entities;

namespace MyndMapper.DTOs.UserDTOs;

public class UserGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public List<Canvas> CreatedCanvases { get; set; } = [];
}
