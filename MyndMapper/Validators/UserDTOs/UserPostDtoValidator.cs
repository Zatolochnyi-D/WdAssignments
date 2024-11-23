using FluentValidation;
using MyndMapper.DTOs.UserDTOs;

namespace MyndMapper.Validators.CanvasDTOs;

public class UserPostDtoValidator : AbstractValidator<UserPostDto>
{
    public UserPostDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}