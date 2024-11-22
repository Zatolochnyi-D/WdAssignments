using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IUserRepository repository) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        User? user = await repository.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(user);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<User> users = await repository.GetAllAsync().AsNoTracking().ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create(User user)
    {
        await repository.AddAsync(user);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit(User user)
    {
        await repository.EditAsync(user);
        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(User user)
    {
        await repository.RemoveAsync(user);
        return Ok();
    }

    [HttpDelete("users/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveAll()
    {
        await repository.RemoveAllAsync();
        return Ok();
    }
}