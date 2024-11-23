using Microsoft.AspNetCore.Authentication.Cookies;
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
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(UserIdDto idDto)
    {
        User? user = await repository.GetAsync(idDto.TargetId);
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<User> users = await repository.GetAllAsync().AsNoTracking().ToListAsync();
        IEnumerable<UserGetDto> getDtos = users.Select(x => new UserGetDto() { Id = x.Id, Name = x.Name, Email = x.Email, Password = x.Password, CreatedCanvases = x.CreatedCanvases });
        return Ok(getDtos);
    }

    [HttpPost]
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

    [HttpPut]
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

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(UserIdDto idDto)
    {
        User user = new()
        {
            Id = idDto.TargetId,
        };
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