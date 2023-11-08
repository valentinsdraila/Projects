using Booklist.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Repositories
{
  public interface IBookCategoryRepository
    {
      void Insert(BookCategory bookCategory);
      Task<bool> SaveChangesAsync();
    }
  public class BookCategoryRepository:IBookCategoryRepository
  {
      private EfDbContext db;
      private DbSet<BookCategory> dbSet;
      public BookCategoryRepository(EfDbContext context)
      {
        db = context;
        dbSet = context.Set<BookCategory>();
      }
      public void Insert(BookCategory bookCategory)
      {
        if (db.Entry(bookCategory).State == EntityState.Detached)
        {
          db.Attach(bookCategory);
          db.Entry(bookCategory).State = EntityState.Added;
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
