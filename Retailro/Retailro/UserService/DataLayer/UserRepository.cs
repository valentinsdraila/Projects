using Microsoft.EntityFrameworkCore;
using UserService.Model;

namespace UserService.Data
{
    public class UserRepository : IUserRepository
    {
        public UserDbContext context { get; set; }
        public UserRepository(UserDbContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task Add(User entity)
        {
            await this.context.Set<User>().AddAsync(entity);
            await this.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var entity = await this.GetById(id);
            if (entity != null)
            {
                this.context.Set<User>().Remove(entity);
                await this.SaveChangesAsync();
            }
        }

        public async Task Delete(User entity)
        {
            this.context.Set<User>().Attach(entity);
            this.context.Set<User>().Remove(entity);
            await this.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await this.context.Set<User>().ToListAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            return await this.context.Set<User>().FindAsync(id);
        }

        public async Task Update(User entity)
        {
            this.context.Entry<User>(entity).State = EntityState.Modified;
            this.context.Entry<User>(entity).Property(x => x.CreatedAt).IsModified = false;
            await this.SaveChangesAsync();
        }

        public async Task MakeAdmin(User user)
        {
            user.Role ="Admin";
            await this.Update(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var savedChanges = await context.SaveChangesAsync();
                return savedChanges > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await this.context.Set<User>().Where(u=>u.Username==username).FirstOrDefaultAsync();
        }
    }
}