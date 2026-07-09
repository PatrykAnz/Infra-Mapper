using System.ComponentModel.DataAnnotations;

namespace InfraMapper.DTOs;

public class TaskCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}
