namespace MyndMapper.DTOs.UserDTOs;

public class UserPutDto
{
    public int TargetId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}