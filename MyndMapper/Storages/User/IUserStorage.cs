using MyndMapper.Entities;

namespace MyndMapper.Storages;

public interface IUserStorage
{
    public User Get(int id);

    public IEnumerable<User> GetAll();

    public void Create(User user);

    public void Edit(int id, User user);

    public void Remove(int id);
}