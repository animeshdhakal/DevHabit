using DevHabit.Api.DTOs.Habits;
using DevHabit.Api.Entities;

namespace DevHabit.Api.Mappers;

public static class HabitMapper
{
    public static Habit ToEntity(this CreateHabitDto createHabitDto)
    {
        return new Habit()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = createHabitDto.Name,
            Description = createHabitDto.Description,
            Type = createHabitDto.Type,
            Frequency = new Frequency()
            {
                Type = createHabitDto.Frequency.Type,
                TimesPerPeriod = createHabitDto.Frequency.TimesPerPeriod,
            },
            Target = new Target()
            {
                Unit = createHabitDto.Target.Unit,
                Value = createHabitDto.Target.Value
            },
            Status = HabitStatus.Ongoing,
            IsArchived = false,
            EndDate = createHabitDto.EndDate,
            Milestone = createHabitDto.Milestone is not null
                ? new Milestone()
                {
                    Target = createHabitDto.Milestone.Target,
                    Current = 0
                }
                : null,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static HabitWithTagsDto ToHabitWithTagsDto(this Habit habit)
    {
        return new HabitWithTagsDto()
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            Type = habit.Type,
            Frequency = new FrequencyDto()
            {
                Type = habit.Frequency.Type,
                TimesPerPeriod = habit.Frequency.TimesPerPeriod,
            },
            Target = new TargetDto()
            {
                Unit = habit.Target.Unit,
                Value = habit.Target.Value
            },
            Status = habit.Status,
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            Milestone = habit.Milestone is not null
                ? new MilestoneDto()
                {
                    Target = habit.Milestone.Target,
                    Current = 0
                }
                : null,
            CreatedAtUtc = habit.CreatedAtUtc,
            UpdatedAtUtc = habit.UpdatedAtUtc,
            LastCompletedAtUtc = habit.LastCompletedAtUtc, 
            Tags = habit.Tags.Select(x => x.Name).ToList()
        };
    }

    public static HabitDto ToDto(this Habit habit)
    {
        return new HabitDto()
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            Type = habit.Type,
            Frequency = new FrequencyDto()
            {
                Type = habit.Frequency.Type,
                TimesPerPeriod = habit.Frequency.TimesPerPeriod,
            },
            Target = new TargetDto()
            {
                Unit = habit.Target.Unit,
                Value = habit.Target.Value
            },
            Status = habit.Status,
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            Milestone = habit.Milestone is not null
                ? new MilestoneDto()
                {
                    Target = habit.Milestone.Target,
                    Current = 0
                }
                : null,
            CreatedAtUtc = habit.CreatedAtUtc,
            UpdatedAtUtc = habit.UpdatedAtUtc,
            LastCompletedAtUtc = habit.LastCompletedAtUtc,
        };
    }

    public static void UpdateFromDto(this Habit habit, UpdateHabitDto updateHabitDto)
    {
        habit.Name = updateHabitDto.Name;
        habit.Description = updateHabitDto.Description;
        habit.Type = updateHabitDto.Type;
        habit.EndDate = updateHabitDto.EndDate;
        habit.Frequency = new Frequency()
        {
            Type = updateHabitDto.Frequency.Type,
            TimesPerPeriod = updateHabitDto.Frequency.TimesPerPeriod,
        };
        habit.Target = new Target()
        {
            Unit = updateHabitDto.Target.Unit,
            Value = updateHabitDto.Target.Value
        };
        if (updateHabitDto.Milestone is not null)
        {
            habit.Milestone ??= new Milestone();
            habit.Milestone.Target = updateHabitDto.Milestone.Target;
        }

        habit.UpdatedAtUtc = DateTime.UtcNow;
    }
}