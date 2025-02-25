using UserService.Data;
using UserService.Model;
namespace UserService.Service;
public class UsersService : IUserService
{

    private readonly IUserRepository userRepository;

    public UsersService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task AddUser(User user)
    {
        var existentUser = await this.userRepository.GetByUsername(user.Username);
        if (existentUser != null)
            return;
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        Console.WriteLine("HashPassword going to db:" + user.Password);
        await this.userRepository.Add(user);
    }

    public async Task DeleteUser(User user)
    {
        await this.userRepository.Delete(user);
    }

    public async Task DeleteUserById(Guid id)
    {
        await this.userRepository.DeleteById(id);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await this.userRepository.GetAll();
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await this.userRepository.GetById(id);
    }

    public async Task UpdateUser(User user)
    {
        await this.userRepository.Update(user);
    }

    public async Task MakeAdmin(User user)
    {
        await this.userRepository.MakeAdmin(user);
    }

    public async Task<User?> AuthenticateUser(string username, string password)
    {
        var user = await userRepository.GetByUsername(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null; // Invalid login
        }
        return user; // Valid login
    }
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

}
