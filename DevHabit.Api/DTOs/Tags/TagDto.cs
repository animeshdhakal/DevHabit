namespace DevHabit.Api.DTOs.Tags;

public record TagsCollectionDto
{
    public required IEnumerable<TagDto> Data { get; init; }
}

public record TagDto
{
    public required string Id { get; init; }
   
    public required string Name { get; init; } 
   
    public string? Description { get; init; }
   
    public required DateTime CreatedAtUtc { get; init; }
   
    public DateTime? UpdatedAtUtc { get; init; } 
}