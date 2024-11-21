using MyndMapper.Entities;

namespace MyndMapper.Storages;

public class UserStorage : IUserStorage
{
    private readonly List<User?> users = [];

    private bool IsValidId(int id)
    {
        if (!(0 <= id && id < users.Count))
        {
            return false;
        }
        if (users[id] == null)
        {
            return false;
        }
        return true;
    }

    public User Get(int id)
    {
        if (IsValidId(id))
        {
            return users[id]!;
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }

    public IEnumerable<User> GetAll()
    {
        return users.Where(x => x != null).Cast<User>();
    }

    public void Create(User user)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i] == null)
            {
                users[i] = user;
                user.Id = i;
                return;
            }
        }
        users.Add(user);
        user.Id = users.Count - 1;
    }

    public void Edit(int id, User user)
    {
        if (IsValidId(id))
        {
            users[id]!.Name = user.Name;
            users[id]!.Email = user.Email;
            users[id]!.Password = user.Password;
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
            users[id] = null;
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }
}