using FluentValidation;
using MyndMapper.DTOs.CanvasDtos;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Validators.CanvasDTOs;

public class CanvasPutDtoValidator : AbstractValidator<CanvasPutDto>
{
    private ICanvasRepository repository;

    public CanvasPutDtoValidator(ICanvasRepository repository)
    {
        this.repository = repository;

        RuleFor(x => x.Id).NotEmpty().Custom(async (id, context) => 
        {
            bool exists = await repository.IsIdExist(id);
            if (!exists)
            {
                context.AddFailure($"Canvas with ID {id} does not exist");
            }
        });
        RuleFor(x => x.Name).NotEmpty();
    }
}