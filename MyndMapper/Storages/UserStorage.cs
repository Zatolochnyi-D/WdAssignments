using MyndMapper.Entities;

namespace MyndMapper.Objects;

public class UserStorage
{
    private static readonly List<UserStorage?> users = [];

    public static void CreateUser(User userModel)
    {
        UserStorage user = new()
        {
            Name = userModel.Name,
            Email = userModel.Email,
            Password = userModel.Password,
            CreatedCanvases = userModel.CreatedCanvases,
        };
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

    public static UserStorage GetUserById(int id)
    {
        if (IsValidId(id))
        {
#pragma warning disable CS8603 // Possible null reference return.
            return users[id];
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            throw new ArgumentException("Invalid ID.");
        }
    }

    public static UserStorage[] GetAllUsers()
    {
        return users.Where(x => x != null).Cast<UserStorage>().ToArray();
    }

    public static void DeleteUserById(int id)
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

    public static bool IsValidId(int id)
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

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public List<int> CreatedCanvases { get; private set; }

    private UserStorage()
    {
        Id = 0;
        Name = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        CreatedCanvases = [];
    }

    public void Update(User userModel)
    {
        Name = userModel.Name;
        Email = userModel.Email;
        Password = userModel.Password;
    }
}