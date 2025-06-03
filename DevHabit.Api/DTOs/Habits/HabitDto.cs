using DevHabit.Api.Entities;

namespace DevHabit.Api.DTOs.Habits;


public record HabitsCollectionDto
{
    public required List<HabitDto> Data { get; init; } 
}

public record HabitWithTagsDto : HabitDto
{
    public required List<string> Tags { get; init; }
}

public record HabitDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitStatus Status { get; init; }
    public required HabitType Type { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public required bool IsArchived { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneDto? Milestone { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public DateTime? LastCompletedAtUtc { get; init; }
}

public record FrequencyDto
{
    public required FrequencyType Type { get; init; }
    public required int TimesPerPeriod { get; init; }
}

public record TargetDto
{
    public required int Value { get; init; }
    public required string Unit { get; init; }
}

public record MilestoneDto
{
    public required int Current { get; init; }
    public required int Target { get; init; }
}