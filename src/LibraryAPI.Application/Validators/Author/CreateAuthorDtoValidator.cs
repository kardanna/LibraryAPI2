using FluentValidation;
using LibraryAPI.Application.DTO.Author;

namespace LibraryAPI.Application.Validators.Author;

public class CreateAuthorDtoValidator : AbstractValidator<CreateAuthorDto>
{
    public CreateAuthorDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(a => a.DateOfBirth)
            .NotNull()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .GreaterThanOrEqualTo(new DateOnly(1000, 1, 1));
    }
}