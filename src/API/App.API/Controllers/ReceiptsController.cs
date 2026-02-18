using System.Security.Claims;
using App.API.Adapters;
using App.Application.Common;
using App.Application.Features.Receipts.Commands;
using App.Application.Features.Receipts.Commands.CreateReceiptViaOcrCommand;
using App.Application.Features.Receipts.Commands.CreateReceiptViaQrCommand;
using App.Application.Features.Receipts.Commands.DeleteReceiptCommand;
using App.Application.Features.Receipts.Commands.ToggleFavoriteCommand;
using App.Application.Features.Receipts.Dtos;
using App.Application.Features.Receipts.Queries;
using App.Application.Features.Receipts.Queries.GetReceiptDetailQuery;
using App.Application.Features.Receipts.Queries.GetReceiptsPagedQuery;
using App.Application.Features.Receipts.Queries.SearchReceiptsQuery;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReceiptsController(IMediator mediator) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES PAGED RECEIPTS FOR THE CURRENT USER.
    /// GET /api/v1/receipts?Page=1&amp;PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetReceiptsPaged([FromQuery] PagedRequest request) 
        => ActionResultInstance(await mediator.Send(new GetReceiptsPagedQuery(UserId, Page: request.Page, PageSize: request.PageSize)));
    

    /// <summary>
    /// RETRIEVES RECEIPT DETAIL WITH ITEMS AND NAVIGATION.
    /// GET /api/v1/receipts/{id}
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceiptDetail(string id) 
        => ActionResultInstance(await mediator.Send(new GetReceiptDetailQuery(UserId, id)));
    

    /// <summary>
    /// SEARCHES RECEIPTS BY TERM (ORGANIZATION NAME, ITEM NAME, CATEGORY, BRAND).
    /// GET /api/v1/receipts/search?searchTerm=market&amp;Page=1&amp;PageSize=10
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> SearchReceipts([FromQuery] string searchTerm, [FromQuery] PagedRequest request) 
        => ActionResultInstance(await mediator.Send(new SearchReceiptsQuery(UserId, searchTerm, request.Page, request.PageSize)));
    

    /// <summary>
    /// CREATES A RECEIPT VIA OCR (IMAGE PROCESSING).
    /// POST /api/v1/receipts/ocr + FORM DATA
    /// </summary>
    [HttpPost("ocr")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateReceiptViaOcr([FromForm] IFormFile imageFile) 
        => ActionResultInstance(await mediator.Send(new CreateReceiptViaOcrCommand(UserId, ImageFile: new FormFileUploadAdapter(imageFile))));
    

    /// <summary>
    /// CREATES A RECEIPT VIA QR CODE.
    /// POST /api/v1/receipts/qr
    /// </summary>
    [HttpPost("qr")]
    public async Task<IActionResult> CreateReceiptViaQr([FromBody] CreateReceiptViaQrRequest request) 
        => ActionResultInstance(await mediator.Send(new CreateReceiptViaQrCommand(UserId, request.QrCodeData)));
    

    /// <summary>
    /// TOGGLES FAVORITE STATUS OF A RECEIPT.
    /// PATCH /api/v1/receipts/{id}/favorite
    /// </summary>
    [HttpPatch("{id}/favorite")]
    public async Task<IActionResult> ToggleFavorite(string id) 
        => ActionResultInstance(await mediator.Send(new ToggleFavoriteCommand(UserId, ReceiptId: id)));
    

    /// <summary>
    /// DELETES A RECEIPT BY ID.
    /// DELETE /api/v1/receipts/{id}
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReceipt(string id) 
        => ActionResultInstance(await mediator.Send(new DeleteReceiptCommand(UserId, ReceiptId: id)));
    
}


