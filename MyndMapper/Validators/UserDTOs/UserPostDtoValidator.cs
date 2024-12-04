using FluentValidation;
using Microsoft.Extensions.Options;
using MyndMapper.Configurations.Configurations;
using MyndMapper.DTOs.UserDTOs;

namespace MyndMapper.Validators.UserDTOs;

public class UserPostDtoValidator : AbstractValidator<UserPostDto>
{
    public UserPostDtoValidator(IOptions<Global> options)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(options.Value.PasswordMinLength);
    }
}