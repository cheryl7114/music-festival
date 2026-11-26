using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class RegisterOrganiserDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 15)]
    public string Password { get; set; } = string.Empty;
}
