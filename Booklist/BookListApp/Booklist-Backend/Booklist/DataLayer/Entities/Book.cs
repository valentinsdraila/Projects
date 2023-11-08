using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Booklist.DataLayer.Entities
{
    public class Book
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public int Year { get; set; }
        [Range(0, 5)]
        public float Rating { get; set; }
        public string Author { get; set; }
    }
}
