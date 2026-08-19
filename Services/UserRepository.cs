using UserManagementApi.Models;

namespace UserManagementApi.Services;

public class UserRepository
{
    private readonly List<User> _users =
    [
        new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        }
    ];

    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
    }

    public User Add(User user)
    {
        if (_users.Any(u =>
            u.Email.Equals(user.Email,
            StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "Email address already exists.");
        }

        user.Id = _users.Count == 0
            ? 1
            : _users.Max(x => x.Id) + 1;

        _users.Add(user);

        return user;
    }

    public bool Update(int id, User user)
    {
        var existing = GetById(id);

        if (existing == null)
            return false;

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Email = user.Email;

        return true;
    }

    public bool Delete(int id)
    {
        var user = GetById(id);

        if (user == null)
            return false;

        _users.Remove(user);

        return true;
    }
}