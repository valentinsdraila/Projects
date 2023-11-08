using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Entities
{
  public class BookCategory
  {
    public Guid ID { get; set; }
    public Guid CategoryID{ get; set; }

    public Category Category { get; set; }

    public Guid BookId { get; set; }

    public Book Book { get; set; }
  }
}
