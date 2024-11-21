using Microsoft.AspNetCore.Mvc;
using MyndMapper.Entities;
using MyndMapper.Storages.UserStorage;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IUserStorage userStorage) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> Get(int id)
    {
        User user;
        try
        {
            user = userStorage.Get(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        IEnumerable<User> users = userStorage.GetAll();
        return Ok(users);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult CreateUser(User user)
    {
        userStorage.Create(user);
        return Created();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Edit(int id, User user)
    {
        try
        {
            userStorage.Edit(id, user);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Remove(int id)
    {
        try
        {
            userStorage.Remove(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return NoContent();
    }
}