using System.ComponentModel.DataAnnotations;

namespace InfraMapper.Models;

public class CloudTask
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}
