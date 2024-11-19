using MyndMapper.Models;

namespace MyndMapper.Objects;

public class UserAccount
{
    private static readonly List<UserAccount?> users = [];

    public static void CreateUser(UserModel userModel)
    {
        UserAccount user = new()
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

    public static UserAccount GetUserById(int id)
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

    public static UserAccount[] GetAllUsers()
    {
        return users.Where(x => x != null).Cast<UserAccount>().ToArray();
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

    private UserAccount()
    {
        Id = 0;
        Name = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        CreatedCanvases = [];
    }

    public void Update(UserModel userModel)
    {
        Name = userModel.Name;
        Email = userModel.Email;
        Password = userModel.Password;
    }
}