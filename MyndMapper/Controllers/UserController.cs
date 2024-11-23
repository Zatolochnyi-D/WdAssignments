using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IUserRepository repository, IMapper mapper) : ControllerBase
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
            UserGetDto getDto = mapper.Map<UserGetDto>(user);
            return Ok(getDto);
        }
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<User> users = await repository.GetAllAsync().AsNoTracking().ToListAsync();
        IEnumerable<UserGetDto> getDtos = users.Select(mapper.Map<UserGetDto>);
        return Ok(getDtos);
    }

    [HttpPost("/create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create(UserPostDto postDto)
    {
        User user = mapper.Map<User>(postDto);
        await repository.AddAsync(user);
        return Ok();
    }

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit(UserPutDto putDto)
    {
        User user = mapper.Map<User>(putDto);
        bool exists = await repository.IsIdExist(putDto.Id);
        if (!exists)
        {
            return NotFound();
        }
        await repository.EditAsync(user);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        User? user = await repository.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
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