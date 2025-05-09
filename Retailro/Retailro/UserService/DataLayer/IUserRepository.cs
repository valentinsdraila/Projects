using UserService.Model;
namespace UserService.Data
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<User> GetById(Guid id);

        /// <summary>Gets all users from the DbSet.</summary>
        /// <returns>
        /// A list of the users.
        /// </returns>
        Task<List<User>> GetAll();

        /// <summary>Adds the specified user to the DbSet.</summary>
        /// <param name="entity">The user.</param>
        Task Add(User entity);

        /// <summary>Updates the specified user.</summary>
        /// <param name="entity">The user.</param>
        Task Update(User entity);

        /// <summary>Deletes the user by identifier.</summary>
        /// <param name="id">The identifier.</param>
        Task DeleteById(Guid id);

        /// <summary>Deletes the specified user.</summary>
        /// <param name="entity">The user.</param>
        Task Delete(User entity);
        /// <summary>
        /// Makes the user admin.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task MakeAdmin(User user);

        /// <summary>Saves the changes.</summary>
        /// <returns>
        ///   The number of state entries written to the database.
        /// </returns>
        Task<bool> SaveChangesAsync();
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        Task<User?> GetByUsername(string username);
    }
}