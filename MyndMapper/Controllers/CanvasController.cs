using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController(DataModelContext context) : ControllerBase
{
    [HttpGet("canvasId={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        Canvas? canvas = await context.Canvases.FindAsync(id);
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
        IEnumerable<Canvas> canvases = await context.Canvases.AsNoTracking().ToListAsync();
        return Ok(canvases);
    }

    [HttpPost("{creatorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(int creatorId, Canvas canvas)
    {
        User? user = await context.Users.FindAsync(creatorId);
        if (user == null)
        {
            return NotFound();
        }
        canvas.Owner = user;
        user.CreatedCanvases.Add(canvas);
        context.Canvases.Add(canvas);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit(int id, Canvas canvas)
    {
        Canvas? OldCanvas = await context.Canvases.FindAsync(id);
        if (OldCanvas != null)
        {
            OldCanvas.Name = canvas.Name;
            await context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        Canvas? canvas = await context.Canvases.FindAsync(id);
        if (canvas != null)
        {
            context.Canvases.Remove(canvas);
            await context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveAll()
    {
        await context.Canvases.ExecuteDeleteAsync();
        return Ok();
    }
}