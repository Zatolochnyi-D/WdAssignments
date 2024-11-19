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
        UserAccount.CreateUser(userModel);
        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserModel> GetUser(int id)
    {
        UserAccount user;
        try
        {
            user = UserAccount.GetUserById(id);
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
        UserAccount[] users = UserAccount.GetAllUsers();
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
        UserAccount user;
        try
        {
            user = UserAccount.GetUserById(id);
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
            UserAccount user = UserAccount.GetUserById(id);
            foreach (var canvasId in user.CreatedCanvases)
            {
                Canvas.DeleteCanvasById(canvasId);
            }
            UserAccount.DeleteUserById(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}