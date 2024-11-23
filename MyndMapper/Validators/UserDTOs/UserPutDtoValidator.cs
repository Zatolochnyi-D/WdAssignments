using FluentValidation;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Validators.CanvasDTOs;

public class UserPutDtoValidator : AbstractValidator<UserPutDto>
{
    private IUserRepository repository;

    public UserPutDtoValidator(IUserRepository repository)
    {
        this.repository = repository;

        RuleFor(x => x.Id).NotNull().Custom(async (id, context) =>
        {
            bool exists = await repository.IsIdExist(id);
            if (!exists)
            {
                context.AddFailure($"Canvas with ID {id} does not exist");
            }
        }); ;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}