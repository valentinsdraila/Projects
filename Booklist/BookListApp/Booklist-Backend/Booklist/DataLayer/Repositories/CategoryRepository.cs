using Booklist.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Repositories
{
  public interface ICategoryRepository
  {
    void Insert(Category category);
    
    Task<bool> SaveChangesAsync();
  }
  public class CategoryRepository : ICategoryRepository
  {
    private EfDbContext db;
    private DbSet<Category> dbSet;
    public CategoryRepository(EfDbContext context)
    {
      db = context;
      dbSet = context.Set<Category>();
    }

    public void Insert(Category category)
    {

      if (db.Entry(category).State == EntityState.Detached)
      {
        db.Attach(category);
        db.Entry(category).State = EntityState.Added;
      }
    }
    public async Task<bool> SaveChangesAsync()
    {
      try
      {
        var savedChanges = await db.SaveChangesAsync();
        return savedChanges > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
      }
    }
  }
}
