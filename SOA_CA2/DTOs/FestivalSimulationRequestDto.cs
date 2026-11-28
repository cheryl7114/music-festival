using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class FestivalSimulationRequestDto : IValidatableObject
{
    [Required]
    [Range(typeof(decimal), "0", "999999999.99")]
    public decimal? TicketPrice { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int? ExpectedTicketsSold { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TicketPrice is decimal price && decimal.Round(price, 2) != price)
        {
            yield return new ValidationResult("Ticket price must have at most two decimal places.",
                [nameof(TicketPrice)]);
        }
    }
}
