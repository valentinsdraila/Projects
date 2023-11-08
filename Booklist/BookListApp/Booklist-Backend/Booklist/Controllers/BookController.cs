using Booklist.DataLayer.Entities;
using Booklist.DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.Controllers
{
  [ApiController]
  [Route("api/books")] //localhost:5000/api/books
  public class BookController : ControllerBase
  {


    private IBookRepository bookRepository;
    public BookController(IBookRepository repository)
    {
      bookRepository = repository;
    }
    [HttpPost]
    [Route("add")] //localhost:5000/api/Books/add
    public async Task<ActionResult<bool>> Add([FromBody] Book request)
    {
      var book = new Book()
      {
        Author = request.Author,
        Description = request.Description,
        Year = request.Year,
        Title = request.Title,
        Rating = request.Rating,
      };

      bookRepository.Insert(book);

      var saveResult = await bookRepository.SaveChangesAsync();

      return Ok(saveResult);
    }
    [HttpGet]
    [Route("get/all")]
    public ActionResult<List<Book>> GetAllBooks()
    {
      var books = bookRepository.GetAllBooks();
      return Ok(books);
    }
    [HttpGet]
    [Route("get/category")]
    public ActionResult<List<Book>> GetBooksByCategory([FromQuery] string category)
    {
      var books = bookRepository.GetBooksByCategory(category); 
      return Ok(books);
    }
    [HttpGet]
    [Route("delete/all")]
    public async Task<ActionResult<List<User>>> DeleteAll()
    {
      var books = bookRepository.GetAllBooks();
      foreach(var book in books)
      {
        bookRepository.Delete(book);
      }
      await bookRepository.SaveChangesAsync();
      return Ok(books);
    }
    [HttpGet]
    [Route("delete")]
    public async Task<ActionResult<List<User>>> Delete([FromQuery] Guid bookId)
    {
      var books = bookRepository.GetAllBooks();
      foreach (var book in books)
      {
        if(book.ID==bookId)
        bookRepository.Delete(book);
      }
      await bookRepository.SaveChangesAsync();
      return Ok(books);
    }

    [HttpGet]
    [Route("delete/book")]  // /api/books/delete/book
    public async Task<ActionResult<List<User>>> DeleteBook([FromQuery] string title)
    {
        var books = bookRepository.GetAllBooks();

        foreach (var book in books)
        {
        if (book.Title == title)
          bookRepository.Delete(book);
        }
      await bookRepository.SaveChangesAsync();
      return Ok(books);
    }
  }
}
