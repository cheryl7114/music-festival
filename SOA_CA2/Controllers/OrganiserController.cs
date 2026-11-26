using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SOA_CA2.DTOs;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganiserController : ControllerBase
{
    private readonly IOrganiserRepository _repository;
    private readonly IPasswordHasher<Organiser> _passwordHasher;

    public OrganiserController(IOrganiserRepository repository, IPasswordHasher<Organiser> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<ActionResult<OrganiserDto>> Register(
        RegisterOrganiserDto request, CancellationToken cancellationToken)
    {
        var organiser = new Organiser
        {
            Email = NormalizeEmail(request.Email),
            PasswordHash = string.Empty
        };
        organiser.PasswordHash = _passwordHasher.HashPassword(organiser, request.Password);

        if (!await _repository.TryAddAsync(organiser, cancellationToken))
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email is already registered.");
        }

        return StatusCode(StatusCodes.Status201Created, new OrganiserDto(organiser.Id, organiser.Email));
    }

    [HttpPost("login")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginOrganiserDto request, CancellationToken cancellationToken)
    {
        var organiser = await _repository.FindByEmailAsync(NormalizeEmail(request.Email), cancellationToken);
        if (organiser == null || _passwordHasher.VerifyHashedPassword(
                organiser, organiser.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Invalid email or password.");
        }

        var scheme = BearerTokenDefaults.AuthenticationScheme;
        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, organiser.Id.ToString(CultureInfo.InvariantCulture))],
            scheme);

        return SignIn(new ClaimsPrincipal(identity), scheme);
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}
