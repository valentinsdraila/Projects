using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer.Entities
{
    public class User
    {
        public Guid ID { get; set; }
        [MaxLength(50)]
        public string email { get; set; }
        [MaxLength(50)]
        public string password { get; set; }
        [MaxLength(50)]
        public string firstName { get; set; }
        [MaxLength(50)]
        public string lastName { get; set; }
    }
}
