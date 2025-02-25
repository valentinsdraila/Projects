using UserService.Model;
namespace UserService.Data
{
    public interface IUserRepository{
    Task<User> GetById(Guid id);

    /// <summary>Gets all entities from the DbSet.</summary>
    /// <returns>
    /// A list of the entities.
    /// </returns>
    Task<List<User>> GetAll();

    /// <summary>Adds the specified entity to the DbSet.</summary>
    /// <param name="entity">The entity.</param>
    Task Add(User entity);

    /// <summary>Updates the specified entity.</summary>
    /// <param name="entity">The entity.</param>
    Task Update(User entity);

    /// <summary>Deletes the entity by identifier.</summary>
    /// <param name="id">The identifier.</param>
    Task DeleteById(Guid id);

    /// <summary>Deletes the specified entity.</summary>
    /// <param name="entity">The entity.</param>
    Task Delete(User entity);

    Task MakeAdmin(User user);

    /// <summary>Saves the changes.</summary>
    /// <returns>
    ///   The number of state entries written to the database.
    /// </returns>
    Task<bool> SaveChangesAsync();
    Task<User?> GetByUsername(string username);
    }
}