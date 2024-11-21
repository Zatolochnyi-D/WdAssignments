namespace MyndMapper.Entities;

public class Canvas
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int OwnerId { get; set; }

    public DateTime CreationDate { get; set; }
}