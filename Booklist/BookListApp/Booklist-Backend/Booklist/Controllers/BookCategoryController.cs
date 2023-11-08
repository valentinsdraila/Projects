using Booklist.DataLayer;
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
  [Route("api/bookcategory")]
  public class BookCategoryController : ControllerBase
  {


    private IBookCategoryRepository bookCategoryRepository;
    public BookCategoryController(IBookCategoryRepository repository)
    {
      bookCategoryRepository = repository;
    }
    [HttpPost]
    [Route("add")] //localhost:5000/api/Books/add
    public async Task<ActionResult<bool>> Add([FromBody] BookCategoryDTO bookCategory)
    {
      var bookCategory1 = new BookCategory()
      {
        BookId = bookCategory.bookId,
        CategoryID = bookCategory.categoryId,
      };

      bookCategoryRepository.Insert(bookCategory1);

      var saveResult = await bookCategoryRepository.SaveChangesAsync();

      return Ok(saveResult);
    }
  }
}
