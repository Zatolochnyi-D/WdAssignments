using MyndMapper.Entities;

namespace MyndMapper.Storages;

public interface ICanvasStorage
{
    public Canvas Get(int id);

    public IEnumerable<Canvas> GetAll();

    public void Create(int creatorId, Canvas canvas);

    public void Edit(int id, Canvas canvas);

    public void Remove(int id);
}