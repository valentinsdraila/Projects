using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Model
{
    public class Users : IEquatable
    {
        public Users()
        {
        }

        public string username { get; set; }
        public string image { get; set; }
    }
}
