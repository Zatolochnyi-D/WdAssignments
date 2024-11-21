namespace MyndMapper.Entities;

public class User
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public List<int> CreatedCanvases { get; set; } = [];
}