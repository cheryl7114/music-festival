using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class FestivalRequestDto : IValidatableObject
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public DateOnly? StartDate { get; set; }

    [Required]
    public DateOnly? EndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "End date must be on or after the start date.", [nameof(EndDate)]);
        }
    }
}
