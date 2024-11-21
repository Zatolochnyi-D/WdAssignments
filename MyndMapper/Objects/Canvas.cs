using System.ComponentModel.DataAnnotations;
using MyndMapper.Objects;

namespace MyndMapper.Models;

public class Canvas
{
    public static Canvas CreateFromCanvas(CanvasStorage canvas)
    {
        Canvas canvasModel = new()
        {
            Id = canvas.Id,
            Name = canvas.Name,
            OwnerId = canvas.OwnerId,
            CreationDate = canvas.CreationDate,
        };
        return canvasModel;
    }

    public int? Id { get; private set; }

    [MinLength(3)]
    public required string Name { get; set; }

    public int? OwnerId { get; private set; }

    public DateTime CreationDate { get; set; }

    public void SetOwnerId(int id)
    {
        OwnerId = id;
    }
}