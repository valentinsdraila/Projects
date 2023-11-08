using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer
{
  public class BookCategoryDTO
  {
    public Guid bookId { get; set; }
    public Guid categoryId { get; set; }
  }
}
