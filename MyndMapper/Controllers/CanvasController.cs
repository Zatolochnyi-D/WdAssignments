using Microsoft.AspNetCore.Mvc;
using MyndMapper.Models;
using MyndMapper.Objects;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController : ControllerBase
{
    [HttpPost("{creatorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult CreateCanvas(int creatorId, CanvasModel canvasModel)
    {
        User user;
        try
        {
            user = Objects.User.GetUserById(creatorId);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        canvasModel.SetOwnerId(creatorId);
        Canvas canvas = Canvas.CreateCanvas(canvasModel);
        user.CreatedCanvases.Add(canvas.Id);
        return Ok();
    }

    [HttpGet("canvasId={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CanvasModel> GetCanvas(int id)
    {
        Canvas canvas;
        try
        {
            canvas = Canvas.GetCanvasById(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        return CanvasModel.CreateFromCanvas(canvas);
    }

    [HttpGet("userId={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CanvasModel[]> GetCanvasesOfUser(int id)
    {
        User user;
        try
        {
            user = Objects.User.GetUserById(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        List<CanvasModel> canvasModels = [];
        foreach (var canvasId in user.CreatedCanvases)
        {
            canvasModels.Add(CanvasModel.CreateFromCanvas(Canvas.GetCanvasById(canvasId)));
        }
        return canvasModels.ToArray();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<CanvasModel[]> GetAllCanvases()
    {
        Canvas[] canvases = Canvas.GetAllCanvases();
        List<CanvasModel> canvasModels = [];
        foreach (var canvas in canvases)
        {
            canvasModels.Add(CanvasModel.CreateFromCanvas(canvas));
        }
        return canvasModels.ToArray();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult EditCanvas(int id, CanvasModel canvasModel)
    {
        Canvas canvas;
        try
        {
            canvas = Canvas.GetCanvasById(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        canvas.Update(canvasModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteCanvasById(int id)
    {
        try
        {
            Canvas canvas = Canvas.GetCanvasById(id);
            Objects.User.GetUserById(canvas.OwnerId).CreatedCanvases.Remove(id);
            Canvas.DeleteCanvasById(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}