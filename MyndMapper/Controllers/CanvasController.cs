using Microsoft.AspNetCore.Mvc;
using MyndMapper.Entities;
using MyndMapper.Objects;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController : ControllerBase
{
    // [HttpPost("{creatorId}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public ActionResult CreateCanvas(int creatorId, Canvas canvasModel)
    // {
    //     UserStorage user;
    //     try
    //     {
    //         user = UserStorage.GetUserById(creatorId);
    //     }
    //     catch (ArgumentException)
    //     {
    //         return NotFound();
    //     }
    //     CanvasStorage canvas = CanvasStorage.CreateCanvas(creatorId, canvasModel);
    //     user.CreatedCanvases.Add(canvas.Id);
    //     return Ok();
    // }

    // [HttpGet("canvasId={id}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public ActionResult<Canvas> GetCanvas(int id)
    // {
    //     CanvasStorage canvas;
    //     try
    //     {
    //         canvas = CanvasStorage.GetCanvasById(id);
    //     }
    //     catch (ArgumentException)
    //     {
    //         return NotFound();
    //     }
    //     return Canvas.CreateFromCanvas(canvas);
    // }

    // [HttpGet("userId={id}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public ActionResult<Canvas[]> GetCanvasesOfUser(int id)
    // {
    //     UserStorage user;
    //     try
    //     {
    //         user = Objects.UserStorage.GetUserById(id);
    //     }
    //     catch (ArgumentException)
    //     {
    //         return NotFound();
    //     }

    //     List<Canvas> canvasModels = [];
    //     foreach (var canvasId in user.CreatedCanvases)
    //     {
    //         canvasModels.Add(Canvas.CreateFromCanvas(CanvasStorage.GetCanvasById(canvasId)));
    //     }
    //     return canvasModels.ToArray();
    // }

    // [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public ActionResult<Canvas[]> GetAllCanvases()
    // {
    //     CanvasStorage[] canvases = CanvasStorage.GetAllCanvases();
    //     List<Canvas> canvasModels = [];
    //     foreach (var canvas in canvases)
    //     {
    //         canvasModels.Add(Canvas.CreateFromCanvas(canvas));
    //     }
    //     return canvasModels.ToArray();
    // }

    // [HttpPut("{id}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public ActionResult EditCanvas(int id, Canvas canvasModel)
    // {
    //     CanvasStorage canvas;
    //     try
    //     {
    //         canvas = CanvasStorage.GetCanvasById(id);
    //     }
    //     catch (ArgumentException)
    //     {
    //         return NotFound();
    //     }
    //     canvas.Update(canvasModel);
    //     return NoContent();
    // }

    // [HttpDelete("{id}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public ActionResult DeleteCanvasById(int id)
    // {
    //     try
    //     {
    //         CanvasStorage canvas = CanvasStorage.GetCanvasById(id);
    //         Objects.UserStorage.GetUserById(canvas.OwnerId).CreatedCanvases.Remove(id);
    //         CanvasStorage.DeleteCanvasById(id);
    //         return NoContent();
    //     }
    //     catch (ArgumentException)
    //     {
    //         return NotFound();
    //     }
    // }
}