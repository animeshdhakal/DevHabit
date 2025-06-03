using DevHabit.Api.Database;
using DevHabit.Api.DTOs.Habits;
using DevHabit.Api.Entities;
using DevHabit.Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits")]
public sealed class HabitsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<HabitsCollectionDto>> GetHabits()
    {
        var habits = await dbContext.Habits.Select(x => x.ToDto()).ToListAsync();
        return Ok(new HabitsCollectionDto { Data = habits });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HabitWithTagsDto>> GetHabit([FromRoute] string id)
    {
        var habit = await dbContext.Habits.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        if (habit is null)
        {
            return NotFound();
        }

        return Ok(habit.ToHabitWithTagsDto());
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHabit([FromRoute] string id,
        [FromBody] UpdateHabitDto updateHabitDto)
    {
        var habit = await dbContext.Habits.FirstOrDefaultAsync(x => x.Id == id);
        if (habit is null)
        {
            return NotFound();
        }

        habit.UpdateFromDto(updateHabitDto);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }


    [HttpPost]
    public async Task<ActionResult> CreateHabit(CreateHabitDto createHabitDto)
    {
        var habit = createHabitDto.ToEntity();
        dbContext.Habits.Add(habit);
        await dbContext.SaveChangesAsync();
        var habitDto = habit.ToDto();
        return CreatedAtAction(nameof(GetHabit), new { id = habitDto.Id }, habitDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHabit([FromRoute] string id)
    {
        var habit = await dbContext.Habits.FirstOrDefaultAsync(x => x.Id == id);

        if (habit is null)
        {
            return NotFound();
        }

        dbContext.Habits.Remove(habit);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}