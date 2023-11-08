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
  [Route("api/category")] //localhost:5000/api/categorys
  public class CategoryController : ControllerBase
  {


    private ICategoryRepository categoryRepository;
    public CategoryController(ICategoryRepository repository)
    {
      categoryRepository = repository;
    }
    [HttpPost]
    [Route("add")] //localhost:5000/api/categorys/add
    public async Task<ActionResult<bool>> Add([FromBody] Category request)
    {
      var category = new Category()
      {
        CategoryName = request.CategoryName,

      };

      categoryRepository.Insert(category);

      var saveResult = await categoryRepository.SaveChangesAsync();

      return Ok(saveResult);
    }
  }
}
