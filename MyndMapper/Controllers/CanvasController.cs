using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.DTOs.CanvasDtos;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;
using Microsoft.Extensions.Options;
using MyndMapper.Configurations.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController(ICanvasRepository repository, IUserRepository userRepository, IMapper mapper, IValidator<CanvasPostDto> postValidator, IValidator<CanvasPutDto> putValidator, IOptions<Global> options, IDistributedCache cache, ILogger<CanvasController> logger) : ControllerBase
{
    private const string AllCanvasesCacheKey = "GetAllCanvases";

    private Global global = options.Value;

    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        logger.LogInformation("Initiated get of Canvas {Id}.", id);
        byte[]? cacheData = await cache.GetAsync(AllCanvasesCacheKey);
        CanvasGetDto? getDto;
        if (cacheData != null)
        {
            IEnumerable<CanvasGetDto> getDtos = JsonSerializer.Deserialize<IEnumerable<CanvasGetDto>>(Encoding.UTF8.GetString(cacheData))!;
            getDto = getDtos.FirstOrDefault(x => x.Id == id);
            logger.LogInformation("Looked up Canvas {Id} in cache.", id);
        }
        else
        {
            getDto = mapper.Map<CanvasGetDto>(await repository.GetWithUsersAsync(id));
            logger.LogInformation("Looked up Canvas {Id} in DB.", id);
        }

        if (getDto == null)
        {
            logger.LogInformation("Canvas {Id} not found.", id);
            return NotFound();
        }
        logger.LogInformation("Canvas {Id} found and returned.", id);
        return Ok(getDto);
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        logger.LogInformation("Initiated get of all Canvases.");
        byte[]? cacheData = await cache.GetAsync(AllCanvasesCacheKey);
        IEnumerable<CanvasGetDto> getDtos;
        if (cacheData != null)
        {
            getDtos = JsonSerializer.Deserialize<IEnumerable<CanvasGetDto>>(Encoding.UTF8.GetString(cacheData))!;
            logger.LogInformation("Looked up all Canvases in cache.");
        }
        else
        {
            IEnumerable<Canvas> canvases = await repository.GetAllAsync().ToListAsync();
            getDtos = canvases.Select(mapper.Map<CanvasGetDto>);
            string serialized = JsonSerializer.Serialize(getDtos);
            byte[] encoded = Encoding.UTF8.GetBytes(serialized);
            cache.Set(AllCanvasesCacheKey, encoded, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(global.CacheLifespanSeconds),
            });
            logger.LogInformation("Looked up all Canvases in DB.");
        }
        logger.LogInformation("Canvases gathered and returned.");
        return Ok(getDtos);
    }

    [HttpPost("create/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(CanvasPostDto postDto)
    {
        logger.LogInformation("Initiated creation of Canvas.");
        ValidationResult result = await postValidator.ValidateAsync(postDto);
        if (!result.IsValid)
        {
            string combinedErrorMessage = "";
            foreach (var error in result.Errors)
            {
                combinedErrorMessage += error.ErrorMessage;
                combinedErrorMessage += "\n";
            }
            logger.LogInformation("Canvas is not created due to incorrect input data:\n{errorMessage}", combinedErrorMessage);
            return BadRequest(combinedErrorMessage);
        }

        User? owner = await userRepository.GetAsync(postDto.OwnerId);
        Canvas canvas = mapper.Map<Canvas>(postDto);
        canvas.Owner = owner!;
        canvas.CreationDate = DateTime.Now;
        owner!.CreatedCanvases.Add(canvas);
        await repository.AddAsync(canvas);
        logger.LogInformation("Canvas {Id} created.", canvas.Id);
        return Ok();
    }

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Edit(CanvasPutDto putDto)
    {
        logger.LogInformation("Initiated modification of Canvas {Id}.", putDto.Id);
        ValidationResult result = await putValidator.ValidateAsync(putDto);
        if (!result.IsValid)
        {
            string combinedErrorMessage = "";
            foreach (var error in result.Errors)
            {
                combinedErrorMessage += error.ErrorMessage;
                combinedErrorMessage += "\n";
            }
            logger.LogInformation("Cnvas is not modified due to incorrect input data:\n{errorMessage}", combinedErrorMessage);
            return BadRequest(result.Errors[0].ErrorMessage);
        }

        Canvas canvas = mapper.Map<Canvas>(putDto);
        await repository.EditAsync(canvas);
        logger.LogInformation("Canvas {Id} modified.", putDto.Id);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        logger.LogInformation("Initiated removal of Canvas {Id}.", id);
        Canvas? canvas = await repository.GetAsync(id);
        if (canvas == null)
        {
            logger.LogInformation("Canvas {Id} does not exist.", id);
            return NotFound();
        }
        await repository.RemoveAsync(canvas);
        logger.LogInformation("Removed Canvas {Id}.", id);
        return Ok();
    }

    [HttpDelete("delete/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> RemoveAll()
    {
        logger.LogInformation("Initiated wipe of Canvas table.");
        if (global.AllowDbWipe)
        {
            await repository.RemoveAllAsync();
            logger.LogInformation("Canvas table cleared.");
            return Ok();
        }
        else
        {
            logger.LogInformation("Wipe denied.");
            return StatusCode(403);
        }
    }
}