using DemoApi.Auth;

namespace DemoApi.Users;

public class InMemoryUserRepository
{
    private readonly List<User> _users = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public InMemoryUserRepository()
    {
        Add("Ana Souza", "ana@example.com", PasswordHasher.Hash("Senha@123"));
        Add("Bruno Lima", "bruno@example.com", PasswordHasher.Hash("Senha@123"));
        Add("Carla Mendes", "carla@example.com", PasswordHasher.Hash("Senha@123"));
    }

    public User? GetById(int id)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }

    public User? GetByEmail(string email)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        }
    }

    public bool EmailExists(string email) => GetByEmail(email) is not null;

    public User Add(string name, string email, string passwordHash)
    {
        lock (_lock)
        {
            var user = new User
            {
                Id = _nextId++,
                Name = name,
                Email = email,
                PasswordHash = passwordHash
            };
            _users.Add(user);
            return user;
        }
    }
}
