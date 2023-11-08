using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Entities
{
    public class UserBooks
    {
        public Guid ID { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }
    }
}
