using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHabit.Api.Database;
using DevHabit.Api.DTOs.HabitTags;
using DevHabit.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public class HabitTagsController(ApplicationDbContext dbContext) : ControllerBase 
{
    [HttpPut]
    public async Task<ActionResult> UpsertHabitTags(string habitId, UpsertHabitTagsDto upsertHabitTagsDto)
    {
        var habit = await dbContext.Habits.Include(x => x.HabitTags).FirstOrDefaultAsync(x => x.Id == habitId);

        if (habit == null)
        {
            return NotFound();
        }

        var currentTagIds = habit.HabitTags.Select(x => x.TagId).ToHashSet();

        if (currentTagIds.SetEquals(upsertHabitTagsDto.TagIds))
        {
           return NoContent(); 
        }

        var existingTagIds = await dbContext.Tags.Where(x => upsertHabitTagsDto.TagIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

        if (existingTagIds.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest("One or more tag IDs do not match");
        }
        
        habit.HabitTags.RemoveAll(x => !upsertHabitTagsDto.TagIds.Contains(x.TagId));

        var tagsToAdd = upsertHabitTagsDto.TagIds.Except(currentTagIds).ToList();
        
        habit.HabitTags.AddRange(tagsToAdd.Select(x => new HabitTag()
        {
            TagId = x,
            HabitId = habitId,
            CreatedAt = DateTime.UtcNow
        }));
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteHabitTags(string habitId, string tagId)
    {
        var habitTag = await dbContext.HabitTags.Where(x=>x.HabitId == habitId && x.TagId == tagId).FirstOrDefaultAsync();

        if (habitTag is null)
        {
            return NotFound();
        }

        dbContext.HabitTags.Remove(habitTag);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
}