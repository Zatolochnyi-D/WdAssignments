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
    public ActionResult CreateUser(UserModel userModel)
    {
        Objects.User.CreateUser(userModel);
        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserModel> GetUser(int id)
    {
        User user;
        try
        {
            user = Objects.User.GetUserById(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return UserModel.CreateFromUser(user);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<UserModel[]> GetAllUsers()
    {
        User[] users = Objects.User.GetAllUsers();
        List<UserModel> userModels = [];
        foreach (var user in users)
        {
            userModels.Add(UserModel.CreateFromUser(user));
        }
        return userModels.ToArray();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult EditUser(int id, UserModel userModel)
    {
        User user;
        try
        {
            user = Objects.User.GetUserById(id);
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
            User user = Objects.User.GetUserById(id);
            foreach (var canvasId in user.CreatedCanvases)
            {
                Canvas.DeleteCanvasById(canvasId);
            }
            Objects.User.DeleteUserById(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}