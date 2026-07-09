using System.ComponentModel.DataAnnotations;

namespace InfraMapper.DTOs;

public class TaskReadDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}
