using FluentValidation;
using Microsoft.Extensions.Options;
using MyndMapper.Configurations.Configurations;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Validators.UserDTOs;

public class UserPutDtoValidator : AbstractValidator<UserPutDto>
{
    private IUserRepository repository;

    public UserPutDtoValidator(IUserRepository repository, IOptions<Global> options)
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
        RuleFor(x => x.Password).NotEmpty().MinimumLength(options.Value.PasswordMinLength);
    }
}