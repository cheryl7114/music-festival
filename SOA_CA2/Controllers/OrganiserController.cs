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
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = string.Empty
        };
        organiser.PasswordHash = _passwordHasher.HashPassword(organiser, request.Password);

        if (!await _repository.TryAddAsync(organiser, cancellationToken))
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email is already registered.");
        }

        return StatusCode(StatusCodes.Status201Created, new OrganiserDto(organiser.Id, organiser.Email));
    }
}
