using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.DTOs;

public class LoginOrganiserDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;
}
