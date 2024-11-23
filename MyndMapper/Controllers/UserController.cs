using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IUserRepository repository) : ControllerBase
{
    [HttpGet("get/{id}")]
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
            UserGetDto getDto = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                CreatedCanvases = user.CreatedCanvases,
            };
            return Ok(getDto);
        }
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<User> users = await repository.GetAllAsync().AsNoTracking().ToListAsync();
        IEnumerable<UserGetDto> getDtos = users.Select(x => new UserGetDto() { Id = x.Id, Name = x.Name, Email = x.Email, Password = x.Password, CreatedCanvases = x.CreatedCanvases });
        return Ok(getDtos);
    }

    [HttpPost("/create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create(UserPostDto postDto)
    {
        User user = new()
        {
            Name = postDto.Name,
            Email = postDto.Email,
            Password = postDto.Password,
        };
        await repository.AddAsync(user);
        return Ok();
    }

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit(UserPutDto putDto)
    {
        User user = new()
        {
            Id = putDto.TargetId,
            Name = putDto.Name,
            Email = putDto.Email,
            Password = putDto.Password,
        };
        await repository.EditAsync(user);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        User user = new()
        {
            Id = id,
        };
        await repository.RemoveAsync(user);
        return Ok();
    }

    [HttpDelete("delete/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveAll()
    {
        await repository.RemoveAllAsync();
        return Ok();
    }
}