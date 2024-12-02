using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyndMapper.DTOs.CanvasDtos;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Controllers;

[ApiController]
[Route("canvases/")]
public class CanvasController(ICanvasRepository repository, IUserRepository userRepository, IMapper mapper, IValidator<CanvasPostDto> postValidator, IValidator<CanvasPutDto> putValidator) : ControllerBase
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
    public async Task<ActionResult> RemoveAll()
    {
        await repository.RemoveAllAsync();
        return Ok();
    }
}