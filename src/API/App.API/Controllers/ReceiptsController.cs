using App.Application.Features.Receipts.Commands.CreateReceipt;
using App.Application.Features.Receipts.Queries.GetReceiptById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceiptsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetReceiptByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFail)
        {
            return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReceiptRequest request)
    {
        var command = new CreateReceiptCommand(
            request.StoreName,
            request.TotalAmount,
            request.ReceiptDate
        );

        var result = await _mediator.Send(command);

        if (result.IsFail)
        {
            return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }
}

public record CreateReceiptRequest(
    string StoreName,
    decimal TotalAmount,
    DateTime ReceiptDate
);
