using System.Text;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MyndMapper.Configurations.Configurations;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("users/")]
public class UserController(IUserRepository repository, IMapper mapper, IValidator<UserPostDto> postValidator, IValidator<UserPutDto> putValidator, IOptions<Global> options, IDistributedCache cache) : ControllerBase
{
    private const string AllUsersCacheKey = "GetAllUsers";

    private Global global = options.Value;

    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        byte[]? cacheData = await cache.GetAsync(AllUsersCacheKey);
        UserGetDto? getDto;
        if (cacheData != null)
        {
            IEnumerable<UserGetDto> getDtos = JsonSerializer.Deserialize<IEnumerable<UserGetDto>>(Encoding.UTF8.GetString(cacheData))!;
            getDto = getDtos.FirstOrDefault(x => x.Id == id);
        }
        else
        {
            getDto = mapper.Map<UserGetDto>(await repository.GetWithCanvasesAsync(id));
        }

        if (getDto == null)
        {
            return NotFound();
        }
        return Ok(getDto);
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        byte[]? cacheData = await cache.GetAsync(AllUsersCacheKey);
        IEnumerable<UserGetDto> getDtos;
        if (cacheData != null)
        {
            getDtos = JsonSerializer.Deserialize<IEnumerable<UserGetDto>>(Encoding.UTF8.GetString(cacheData))!;
        }
        else
        {
            IEnumerable<User> users = await repository.GetAllAsync().ToListAsync();
            getDtos = users.Select(mapper.Map<UserGetDto>);
            string serialized = JsonSerializer.Serialize(getDtos);
            byte[] encoded = Encoding.UTF8.GetBytes(serialized);
            cache.Set(AllUsersCacheKey, encoded, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(global.CacheLifespanSeconds),
            });
        }
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> RemoveAll()
    {
        if (global.AllowDbWipe)
        {
            await repository.RemoveAllAsync();
            return Ok();
        }
        else
        {
            return StatusCode(403);
        }
    }
}