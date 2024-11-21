using Microsoft.AspNetCore.Mvc;
using MyndMapper.Models;
using MyndMapper.Objects;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult CreateUser(User userModel)
    {
        Objects.UserStorage.CreateUser(userModel);
        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetUser(int id)
    {
        UserStorage user;
        try
        {
            user = Objects.UserStorage.GetUserById(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return Models.User.CreateFromUser(user);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<User[]> GetAllUsers()
    {
        UserStorage[] users = Objects.UserStorage.GetAllUsers();
        List<User> userModels = [];
        foreach (var user in users)
        {
            userModels.Add(Models.User.CreateFromUser(user));
        }
        return userModels.ToArray();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult EditUser(int id, User userModel)
    {
        UserStorage user;
        try
        {
            user = Objects.UserStorage.GetUserById(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        user.Update(userModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteUserById(int id)
    {
        try
        {
            UserStorage user = Objects.UserStorage.GetUserById(id);
            foreach (var canvasId in user.CreatedCanvases)
            {
                CanvasStorage.DeleteCanvasById(canvasId);
            }
            Objects.UserStorage.DeleteUserById(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}