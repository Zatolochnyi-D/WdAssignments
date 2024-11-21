using MyndMapper.Entities;
using MyndMapper.Storages.UserStorage;

namespace MyndMapper.Storages.CanvasStorage;

public class CanvasStorage(IUserStorage userStorage) : ICanvasStorage
{
    private readonly List<Canvas?> canvases = [];

    private bool IsValidId(int id)
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

    public Canvas Get(int id)
    {
        if (IsValidId(id))
        {
            return canvases[id]!;
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }

    public IEnumerable<Canvas> GetAll()
    {
        return canvases.Where(x => x != null).Cast<Canvas>();
    }

    // TODO No checks provided. Any input data will be acceptable.
    public void Create(int creatorId, Canvas canvas)
    {
        User user = userStorage.Get(creatorId);

        for (int i = 0; i < canvases.Count; i++)
        {
            if (canvases[i] == null)
            {
                canvases[i] = canvas;
                canvas.Id = i;
                return;
            }
        }
        canvases.Add(canvas);
        canvas.Id = canvases.Count - 1;
        user.CreatedCanvases.Add(canvas.Id);
    }

    public void Edit(int id, Canvas canvas)
    {
        if (IsValidId(id))
        {
            canvases[id]!.Name = canvas.Name;
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }

    public void Remove(int id)
    {
        if (IsValidId(id))
        {
            userStorage.Get(canvases[id]!.OwnerId).CreatedCanvases.Remove(id);
            canvases[id] = null;
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }
}