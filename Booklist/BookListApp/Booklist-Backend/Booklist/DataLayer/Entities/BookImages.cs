using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Entities
{
  public class BookImages
  {
    public Guid ID { get; set; }
    public string url { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
  }
}
