using App.Application.Features.AIAnalysis.Commands.AnalyzeReceiptCommand;
using App.Application.Features.AIAnalysis.Dtos;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class AIAnalysisController(IMediator mediator) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// ANALYZES A RECEIPT USING AI WITH THE GIVEN PROMPT.
    /// POST /api/v1/aianalysis
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AnalyzeReceipt([FromBody] AnalyzeReceiptRequest request) 
        => ActionResultInstance(await mediator.Send(new AnalyzeReceiptCommand(UserId, ReceiptId: request.ReceiptId, Prompt: request.Prompt, ProviderName: request.ProviderName)));
    
}
