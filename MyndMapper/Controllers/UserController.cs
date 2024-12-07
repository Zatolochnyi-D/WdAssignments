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
public class UserController(IUserRepository repository, IMapper mapper, IValidator<UserPostDto> postValidator, IValidator<UserPutDto> putValidator, IOptions<Global> options, IDistributedCache cache, ILogger<UserController> logger) : ControllerBase
{
    private const string AllUsersCacheKey = "GetAllUsers";

    private Global global = options.Value;

    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        logger.LogDebug("Initiated Get User {Id}", id);
        byte[]? cacheData = await cache.GetAsync(AllUsersCacheKey);
        UserGetDto? getDto;
        if (cacheData != null)
        {
            IEnumerable<UserGetDto> getDtos = JsonSerializer.Deserialize<IEnumerable<UserGetDto>>(Encoding.UTF8.GetString(cacheData))!;
            getDto = getDtos.FirstOrDefault(x => x.Id == id);
            logger.LogDebug("Looked up User {Id} in cache.", id);
        }
        else
        {
            getDto = mapper.Map<UserGetDto>(await repository.GetWithCanvasesAsync(id));
            logger.LogDebug("Looked up User {Id} in DB.", id);
        }

        if (getDto == null)
        {
            logger.LogDebug("User {Id} not found.", id);
            return NotFound();
        }
        logger.LogDebug("User {Id} found and returned.", id);
        return Ok(getDto);
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        logger.LogDebug("Initiated get of all Users.");
        byte[]? cacheData = await cache.GetAsync(AllUsersCacheKey);
        IEnumerable<UserGetDto> getDtos;
        if (cacheData != null)
        {
            getDtos = JsonSerializer.Deserialize<IEnumerable<UserGetDto>>(Encoding.UTF8.GetString(cacheData))!;
            logger.LogDebug("Looked up Users in cache.");
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
            logger.LogDebug("Looked up Users in DB.");
        }
        logger.LogDebug("All Users gathered and returned.");
        return Ok(getDtos);
    }

    [HttpPost("create/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(UserPostDto postDto)
    {
        logger.LogDebug("Initiated creation of new User.");
        ValidationResult result = await postValidator.ValidateAsync(postDto);
        if (!result.IsValid)
        {
            string combinedErrorMessage = "";
            foreach (var error in result.Errors)
            {
                combinedErrorMessage += error.ErrorMessage;
                combinedErrorMessage += "\n";
            }
            logger.LogDebug("User is not created due to incorrect input data:\n{errorMessage}", combinedErrorMessage);
            return BadRequest(combinedErrorMessage);
        }

        User user = mapper.Map<User>(postDto);
        await repository.AddAsync(user);
        logger.LogDebug("Created new User - {id}", user.Id);
        return Ok();
    }

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Edit(UserPutDto putDto)
    {
        logger.LogDebug("Initiated modification of User {Id}.", putDto.Id);
        ValidationResult result = await putValidator.ValidateAsync(putDto);
        if (!result.IsValid)
        {
            string combinedErrorMessage = "";
            foreach (var error in result.Errors)
            {
                combinedErrorMessage += error.ErrorMessage;
                combinedErrorMessage += "\n";
            }
            logger.LogDebug("User {Id} is not modified due to incorrect input data:\n{errorMessage}", putDto.Id, combinedErrorMessage);
            return BadRequest(combinedErrorMessage);
        }
        User user = mapper.Map<User>(putDto);
        await repository.EditAsync(user);
        logger.LogDebug("Successfuly modified User {Id}", putDto.Id);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        logger.LogDebug("Initiated removal of User {Id}.", id);
        User? user = await repository.GetAsync(id);
        if (user == null)
        {
            logger.LogDebug("User {Id} does not exist.", id);
            return NotFound();
        }
        await repository.RemoveAsync(user);
        logger.LogDebug("Removed User {Id}", id);
        return Ok();
    }

    [HttpDelete("delete/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> RemoveAll()
    {
        logger.LogDebug("Initiated wipe of User table.");
        if (global.AllowDbWipe)
        {
            await repository.RemoveAllAsync();
            logger.LogDebug("User table cleared.");
            return Ok();
        }
        else
        {
            logger.LogDebug("Wipe denied.");
            return StatusCode(403);
        }
    }
}