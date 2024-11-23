namespace MyndMapper.DTOs.CanvasDtos;

public class CanvasGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public int OwnerId { get; set; }
}