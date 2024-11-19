using MyndMapper.Models;

namespace MyndMapper.Objects;

public class Canvas
{
    private static readonly List<Canvas?> canvases = [];
    public static Canvas CreateCanvas(CanvasModel canvasModel)
    {
        if (!canvasModel.OwnerId.HasValue)
        {
            throw new ArgumentException("Incomplete canvas model is given.");
        }
        Canvas canvas = new()
        {
            Name = canvasModel.Name,
            OwnerId = canvasModel.OwnerId.Value,
            CreationDate = canvasModel.CreationDate,
        };
        for (int i = 0; i < canvases.Count; i++)
        {
            if (canvases[i] == null)
            {
                canvases[i] = canvas;
                canvas.Id = i;
                return canvas;
            }
        }
        canvases.Add(canvas);
        canvas.Id = canvases.Count - 1;
        return canvas;
    }
    public static Canvas GetCanvasById(int id)
    {
        if (IsValidId(id))
        {
#pragma warning disable CS8603 // Possible null reference return.
            return canvases[id];
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }
    public static Canvas[] GetAllCanvases()
    {
        return canvases.Where(x => x != null).Cast<Canvas>().ToArray();
    }
    public static void DeleteCanvasById(int id)
    {
        if (IsValidId(id))
        {
            canvases[id] = null;
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }
    public static bool IsValidId(int id)
    {
        if (!(0 <= id && id < canvases.Count))
        {
            return false;
        }
        if (canvases[id] == null)
        {
            return false;
        }
        return true;
    }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int OwnerId { get; private set; }
    public DateTime CreationDate { get; private set; }
    private Canvas()
    {
        Id = default;
        Name = string.Empty;
        OwnerId = default;
        CreationDate = default;
    }
    public void Update(CanvasModel canvasModel)
    {
        Name = canvasModel.Name;
    }
}