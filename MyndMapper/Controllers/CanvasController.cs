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
        Canvas? canvas = await repository.GetWithUsersAsync(id);
        if (canvas == null)
        {
            return NotFound();
        }
        CanvasGetDto getDto = mapper.Map<CanvasGetDto>(canvas);
        return Ok(getDto);
    }

    [HttpGet("get/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Canvas> canvases = await repository.GetAllAsync().ToListAsync();
        IEnumerable<CanvasGetDto> getDtos = canvases.Select(mapper.Map<CanvasGetDto>);
        return Ok(getDtos);
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

    [HttpPut("edit/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit(CanvasPutDto putDto)
    {
        Canvas canvas = mapper.Map<Canvas>(putDto);
        bool exists = await repository.IsIdExist(putDto.Id);
        if (!exists)
        {
            return NotFound();
        }
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
    public async Task<ActionResult> RemoveAll()
    {
        await repository.RemoveAllAsync();
        return Ok();
    }
}