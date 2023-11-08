using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Booklist.DataLayer
{
  public class UserDTO
  {
    [MaxLength(50, ErrorMessage = "Email too long!")]
    public string email { get; set; }

    [MaxLength(50, ErrorMessage = "Password too long!")]
    public string password { get; set; }
  }
}
