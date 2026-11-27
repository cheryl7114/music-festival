using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class PerformerBookingRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int FestivalId { get; set; }

    [Range(1, int.MaxValue)]
    public int PerformerId { get; set; }

    [Required]
    public DateTime? StartsAtUtc { get; set; }

    [Required]
    public DateTime? EndsAtUtc { get; set; }

    [Required]
    [Range(typeof(decimal), "0", "999999999.99")]
    public decimal? Fee { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartsAtUtc?.Kind != DateTimeKind.Utc || EndsAtUtc?.Kind != DateTimeKind.Utc)
        {
            yield return new ValidationResult("Send both times in UTC with a Z suffix.",
                [nameof(StartsAtUtc), nameof(EndsAtUtc)]);
        }

        if (EndsAtUtc <= StartsAtUtc)
        {
            yield return new ValidationResult("End time must be after the start time.", [nameof(EndsAtUtc)]);
        }

        if (Fee is decimal fee && decimal.Round(fee, 2) != fee)
        {
            yield return new ValidationResult("Fee must have at most two decimal places.", [nameof(Fee)]);
        }
    }
}
