using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfraMapper.Data;
using InfraMapper.Models;
using InfraMapper.DTOs;

namespace InfraMapper.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll()
    {
        var tasks = await _context.Tasks
            .AsNoTracking()
            .Select(t => new TaskReadDto
            {
                Id = t.Id,
                Name = t.Name,
                IsCompleted = t.IsCompleted
            })
            .ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskReadDto>> GetById(int id)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TaskReadDto
            {
                Id = t.Id,
                Name = t.Name,
                IsCompleted = t.IsCompleted
            })
            .FirstOrDefaultAsync();

        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskReadDto>> Create(TaskCreateDto taskDto)
    {
        var newTask = new CloudTask
        {
            Name = taskDto.Name,
            IsCompleted = false
        };

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();

        var readDto = new TaskReadDto
        {
            Id = newTask.Id,
            Name = newTask.Name,
            IsCompleted = newTask.IsCompleted
        };

        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, TaskReadDto taskDto)
    {
        if (id != taskDto.Id) return BadRequest("ID mismatch");
        var existing = await _context.Tasks.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = taskDto.Name;
        existing.IsCompleted = taskDto.IsCompleted;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
