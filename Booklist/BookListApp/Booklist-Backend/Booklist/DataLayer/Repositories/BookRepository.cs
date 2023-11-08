using Booklist.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Repositories
{
  public interface IBookRepository
  {
    void Insert(Book book);
    List<Book> GetBooksByCategory(string category);
    List<Book> GetAllBooks();
    Task<bool> SaveChangesAsync();
    void Delete(Book book);
  }
  public class BookRepository : IBookRepository
  {
    private EfDbContext db;
    private DbSet<Book> dbSet;
    public BookRepository(EfDbContext context)
    {
      db = context;
      dbSet = context.Set<Book>();
    }

    public List<Book> GetBooksByCategory(string category)
    {
      
      Guid categoryID = db.Categories.Where(c => c.CategoryName == category).Select(c=>c.ID).FirstOrDefault();
      
      var bookIDs = db.BookCategories.Where(bc => bc.CategoryID == categoryID).Select(bc=>bc.BookId);
      
      var books = db.Books.Where(b=> bookIDs.Contains(b.ID)).ToList();
      return books;
    }

    public void Insert(Book book)
    {
      if (db.Entry(book).State == EntityState.Detached)
      {
        db.Attach(book);
        db.Entry(book).State = EntityState.Added;
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

    public List<Book> GetAllBooks()
    {
      
      var books = db.Books.ToList();
      return books;
    }
    public void Delete(Book record)
    {
      db.Books.Remove(record);
    }

  }
}

