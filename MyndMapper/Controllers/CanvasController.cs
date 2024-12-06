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
public class CanvasController(ICanvasRepository repository, IUserRepository userRepository, IMapper mapper, IValidator<CanvasPostDto> postValidator, IValidator<CanvasPutDto> putValidator, IOptions<Global> options, IDistributedCache cache) : ControllerBase
{
    private const string AllCanvasesCacheKey = "GetAllCanvases";

    private Global global = options.Value;

    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        byte[]? cacheData = await cache.GetAsync(AllCanvasesCacheKey);
        CanvasGetDto? getDto;
        if (cacheData != null)
        {
            IEnumerable<CanvasGetDto> getDtos = JsonSerializer.Deserialize<IEnumerable<CanvasGetDto>>(Encoding.UTF8.GetString(cacheData))!;
            getDto = getDtos.FirstOrDefault(x => x.Id == id);
        }
        else
        {
            getDto = mapper.Map<CanvasGetDto>(await repository.GetWithUsersAsync(id));
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
        byte[]? cacheData = await cache.GetAsync(AllCanvasesCacheKey);
        IEnumerable<CanvasGetDto> getDtos;
        if (cacheData != null)
        {
            getDtos = JsonSerializer.Deserialize<IEnumerable<CanvasGetDto>>(Encoding.UTF8.GetString(cacheData))!;
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
        }
        return Ok(getDtos);
    }

    [HttpPost("create/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(CanvasPostDto postDto)
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

        User? owner = await userRepository.GetAsync(postDto.OwnerId);
        Canvas canvas = mapper.Map<Canvas>(postDto);
        canvas.Owner = owner!;
        canvas.CreationDate = DateTime.Now;
        owner!.CreatedCanvases.Add(canvas);
        await repository.AddAsync(canvas);
        return Ok();
    }

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Edit(CanvasPutDto putDto)
    {
        ValidationResult result = await putValidator.ValidateAsync(putDto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors[0].ErrorMessage);
        }

        Canvas canvas = mapper.Map<Canvas>(putDto);
        await repository.EditAsync(canvas);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        Canvas? canvas = await repository.GetAsync(id);
        if (canvas == null)
        {
            return NotFound();
        }
        await repository.RemoveAsync(canvas);
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