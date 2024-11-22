namespace MyndMapper.Entities;

public class Canvas
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public User Owner { get; set; } = null!;
}