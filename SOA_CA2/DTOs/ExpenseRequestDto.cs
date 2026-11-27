using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class ExpenseRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int FestivalId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(typeof(decimal), "0.01", "999999999.99")]
    public decimal? Amount { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is decimal amount && decimal.Round(amount, 2) != amount)
        {
            yield return new ValidationResult("Amount must have at most two decimal places.", [nameof(Amount)]);
        }
    }
}
