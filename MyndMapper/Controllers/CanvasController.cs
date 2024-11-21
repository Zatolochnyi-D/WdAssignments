using Microsoft.AspNetCore.Mvc;
using MyndMapper.Entities;
using MyndMapper.Storages.CanvasStorage;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController(ICanvasStorage canvasStorage) : ControllerBase
{
    [HttpGet("canvasId={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Canvas> Get(int id)
    {
        Canvas canvas;
        try
        {
            canvas = canvasStorage.Get(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return Ok(canvas);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Canvas[]> GetAll()
    {
        IEnumerable<Canvas> canvases = canvasStorage.GetAll();
        return Ok(canvases);
    }

    [HttpPost("{creatorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Create(int creatorId, Canvas canvas)
    {
        try
        {
            canvasStorage.Create(creatorId, canvas);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Edit(int id, Canvas canvas)
    {
        try
        {
            canvasStorage.Edit(id, canvas);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Remove(int id)
    {
        try
        {
            canvasStorage.Remove(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return NoContent();
    }
}