using MyndMapper.Entities;

namespace MyndMapper.Objects;

public class CanvasStorage
{
    private static readonly List<CanvasStorage?> canvases = [];

    public static CanvasStorage CreateCanvas(int ownerId, Canvas canvasModel)
    {
        CanvasStorage canvas = new()
        {
            Name = canvasModel.Name,
            OwnerId = ownerId,
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

    public static CanvasStorage GetCanvasById(int id)
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

    public static CanvasStorage[] GetAllCanvases()
    {
        return canvases.Where(x => x != null).Cast<CanvasStorage>().ToArray();
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
    private CanvasStorage()
    {
        Id = default;
        Name = string.Empty;
        OwnerId = default;
        CreationDate = default;
    }
    
    public void Update(Canvas canvasModel)
    {
        Name = canvasModel.Name;
    }
}