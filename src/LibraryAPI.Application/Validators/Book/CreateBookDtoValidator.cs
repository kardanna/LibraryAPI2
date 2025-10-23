using FluentValidation;
using LibraryAPI.Application.DTO.Book;

namespace LibraryAPI.Application.Validators.Book;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(b => b.Title)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(b => b.PublishedYear)
            .NotNull()
            .GreaterThanOrEqualTo(1000)
            .LessThanOrEqualTo(DateTime.Now.Year);
            
        RuleFor(b => b.AuthorId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}