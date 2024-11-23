using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.DTOs.CanvasDtos;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController(ICanvasRepository repository, IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        Canvas? canvas = await repository.GetAsync(id);
        if (canvas == null)
        {
            return NotFound();
        }
        return Ok(canvas);
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Canvas> canvases = await repository.GetAllAsync().ToListAsync();
        return Ok(canvases);
    }

    [HttpPost("create/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(CanvasPostDto postDto)
    {
        User? owner = await userRepository.GetAsync(postDto.OwnerId);
        if (owner == null)
        {
            return NotFound();
        }
        Canvas canvas = mapper.Map<Canvas>(postDto);
        canvas.Owner = owner;
        owner.CreatedCanvases.Add(canvas);
        await repository.AddAsync(canvas);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit(Canvas canvas)
    {
        await repository.EditAsync(canvas);
        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(Canvas canvas)
    {
        await repository.RemoveAsync(canvas);
        return Ok();
    }

    [HttpDelete("canvases/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveAll()
    {
        await repository.RemoveAllAsync();
        return Ok();
    }
}