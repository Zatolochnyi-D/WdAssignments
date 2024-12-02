using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IUserRepository repository, IMapper mapper, IValidator<UserPostDto> postValidator, IValidator<UserPutDto> putValidator) : ControllerBase
{
    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        User? user = await repository.GetWithCanvasesAsync(id);
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
        IEnumerable<User> users = await repository.GetAllAsync().ToListAsync();
        IEnumerable<UserGetDto> getDtos = users.Select(mapper.Map<UserGetDto>);
        return Ok(getDtos);
    }

    [HttpPost("create/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(UserPostDto postDto)
    {
        ValidationResult result = await postValidator.ValidateAsync(postDto);
        if (!result.IsValid)
        {
            string combinedErrorMessage = "";
            foreach (var error in result.Errors)
            {
                combinedErrorMessage += error.ErrorMessage;
                combinedErrorMessage += "\n";
            }
            return BadRequest(combinedErrorMessage);
        }
        
        User user = mapper.Map<User>(postDto);
        await repository.AddAsync(user);
        return Ok();
    }

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Edit(UserPutDto putDto)
    {
        ValidationResult result = await putValidator.ValidateAsync(putDto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors[0].ErrorMessage);
        }
        User user = mapper.Map<User>(putDto);
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