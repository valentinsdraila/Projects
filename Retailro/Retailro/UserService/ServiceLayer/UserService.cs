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

    public async Task<bool> AddUser(User user)
    {
        var existentUser = await this.userRepository.GetByUsername(user.Username);
        if (existentUser != null)
            return true;
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await this.userRepository.Add(user);
        return false;
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

    public async Task<UserDTO> GetUser(Guid id)
    {
        var user = await this.userRepository.GetById(id);
        var userDTO = new UserDTO()
        {
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            FirstName = user.FirstName,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            DeliveryAddresses = user.DeliveryAddresses.Select(a => new DeliveryAddressDTO
            {
                Address = a.Address,
                City = a.City,
                County = a.County,
                FullName = a.FullName,
                Id = a.Id,
                ZipCode = a.ZipCode,
            }).ToList()
        };
        return userDTO;
    }

    public async Task UpdateUser(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
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
