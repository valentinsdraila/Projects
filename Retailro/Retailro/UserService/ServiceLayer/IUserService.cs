using UserService.Model;
namespace UserService.Service;

public interface IUserService
{
    /// <summary>Adds the user.</summary>
    /// <param name="user">The user.</param>
    Task<bool> AddUser(User user);

    /// <summary>Updates the user.</summary>
    /// <param name="user">The user.</param>
    Task UpdateUser(User user);

    /// <summary>Gets the user.</summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The user.</returns>
    Task<UserDTO> GetUser(Guid id);

    /// <summary>Deletes the user by identifier.</summary>
    /// <param name="id">The identifier.</param>
    Task DeleteUserById(Guid id);

    /// <summary>Deletes the user.</summary>
    /// <param name="user">The user.</param>
    Task DeleteUser(User user);

    /// <summary>Gets all users.</summary>
    /// <returns>
    ///   <para>A list of all the users in the database.</para>
    /// </returns>
    Task<List<User>> GetAllUsers();

    /// <summary>Makes the user an admin.</summary>
    /// <param name="user">The user.</param>
    Task MakeAdmin(User user);
    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns></returns>
    Task<User?> AuthenticateUser(string username, string password);
}