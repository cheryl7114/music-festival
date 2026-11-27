using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOA_CA2.DTOs;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseRepository _repository;

    public ExpenseController(IExpenseRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(
        ExpenseRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var expense = new Expense
        {
            FestivalId = request.FestivalId,
            Description = request.Description.Trim(),
            Amount = request.Amount!.Value
        };

        if (!await _repository.TryAddAsync(expense, organiserId, cancellationToken))
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Festival not found.");
        }

        return StatusCode(StatusCodes.Status201Created,
            new ExpenseDto(expense.Id, expense.FestivalId, expense.Description, expense.Amount));
    }
}
