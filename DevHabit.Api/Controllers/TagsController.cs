using DevHabit.Api.Database;
using DevHabit.Api.DTOs.Tags;
using DevHabit.Api.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[Route("tags")]
public class TagsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TagsCollectionDto>> GetTags()
    {
        var tagDtos = await dbContext.Tags.Select(x => x.ToDto()).ToListAsync();
        return Ok(new TagsCollectionDto()
        {
            Data = tagDtos
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag([FromRoute] string id)
    {
        var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        if (tag is null)
        {
            return NotFound();
        }

        var tagDto = tag.ToDto();
        return Ok(tagDto);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag([FromBody] CreateTagDto createTagDto,
        [FromServices] IValidator<CreateTagDto> validator)
    {
        await validator.ValidateAndThrowAsync(createTagDto);
        
        if (await dbContext.Tags.AnyAsync(x => x.Name == createTagDto.Name))
        {
            return Problem("Tag with the same name already exists", statusCode: StatusCodes.Status400BadRequest);
        }

        var entity = createTagDto.ToEntity();

        dbContext.Tags.Add(entity);
        await dbContext.SaveChangesAsync();

        var tagDto = entity.ToDto();

        return CreatedAtAction(nameof(GetTags), new { id = tagDto.Id }, tagDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag([FromRoute] string id, [FromBody] UpdateTagDto updateTagDto)
    {
        var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

        if (tag is null)
        {
            return NotFound();
        }

        tag.UpdateFromDto(updateTagDto);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag([FromRoute] string id)
    {
        var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

        if (tag is null)
        {
            return NotFound();
        }

        dbContext.Tags.Remove(tag);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}