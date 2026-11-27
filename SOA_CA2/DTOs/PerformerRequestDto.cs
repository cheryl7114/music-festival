using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class PerformerRequestDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
