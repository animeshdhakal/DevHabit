using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configurations;

public sealed class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(500);

        builder.Property(x => x.Name).HasMaxLength(500);

        builder.Property(x => x.Description).HasMaxLength(500);

        builder.OwnsOne(x => x.Target);

        builder.OwnsOne(x => x.Frequency);

        builder.OwnsOne(x => x.Milestone);

        builder.HasMany(x => x.Tags).WithMany().UsingEntity<HabitTag>();

    }
}