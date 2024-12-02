using FluentValidation;
using MyndMapper.DTOs.CanvasDtos;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Validators.CanvasDTOs;

public class CanvasPostDtoValidator : AbstractValidator<CanvasPostDto>
{
    private IUserRepository repository;

    public CanvasPostDtoValidator(IUserRepository repository)
    {
        this.repository = repository;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.OwnerId).NotNull().Custom(async (id, context) =>
        {
            bool exists = await repository.IsIdExist(id);
            if (!exists)
            {
                context.AddFailure($"Canvas with ID {id} does not exist");
            }
        }); ;
    }
}