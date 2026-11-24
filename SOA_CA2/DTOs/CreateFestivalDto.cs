using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class CreateFestivalDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public DateOnly? StartDate { get; set; }

    [Required]
    public DateOnly? EndDate { get; set; }
}
