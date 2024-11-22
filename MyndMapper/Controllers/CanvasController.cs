using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController(ICanvasRepository repository) : ControllerBase
{
    [HttpGet("canvasId={id}")]
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Canvas> canvases = await repository.GetAllAsync().AsNoTracking().ToListAsync();
        return Ok(canvases);
    }

    [HttpPost("{creatorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(Canvas canvas)
    {
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

    [HttpDelete("/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveAll()
    {
        await repository.RemoveAllAsync();
        return Ok();
    }
}