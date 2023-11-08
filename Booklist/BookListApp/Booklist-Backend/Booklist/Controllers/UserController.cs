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
  [Route("api/users")] //localhost:5000/api/users
  public class UserController : ControllerBase
  {

    
    private IUserRepository userRepository;
    public UserController(IUserRepository repository)
    {
      userRepository = repository;
    }
    [HttpPost]
    [Route("add")] //localhost:5000/api/users/add
    public async Task<ActionResult<bool>> Add([FromBody] User request)
    {
      var user = new User()
      {
        email = request.email,
        password = request.password,
        firstName = request.firstName,
        lastName = request.lastName,

      };

      userRepository.Insert(user);

      var saveResult = await userRepository.SaveChangesAsync();

      return Ok(saveResult);
    }
    [HttpPost]
    [Route("login")] //localhost:44340/api/users/login
    public ActionResult<User> GetUser([FromBody] UserDTO user )
    {
      var user1=userRepository.GetUser(user.email, user.password);
      if (user1 != null)
        return Ok(user1);
      else
        return BadRequest("User was not found!");
    }
    [HttpGet]
    [Route("delete/all")]
    public async Task<ActionResult<List<User>>> DeleteAll()
    {
      var users = userRepository.GetAllUsers();
      foreach (var user in users)
      {
        userRepository.Delete(user);
      }
      await userRepository.SaveChangesAsync();
      return Ok(users);
    }
    [HttpGet]
    [Route("delete")]
    public async Task<ActionResult<List<User>>> Delete([FromQuery] Guid userId)
    {
      var users = userRepository.GetAllUsers();
      foreach (var user in users)
      {
        if (user.ID == userId)
          userRepository.Delete(user);
      }
      await userRepository.SaveChangesAsync();
      return Ok(users);
    }
  }
}
