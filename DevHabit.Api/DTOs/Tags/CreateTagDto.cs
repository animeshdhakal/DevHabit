using FluentValidation;

namespace DevHabit.Api.DTOs.Tags;

public record CreateTagDto
{
    public required string Name { get; init; }

    public string? Description { get; init; }
}


public class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}