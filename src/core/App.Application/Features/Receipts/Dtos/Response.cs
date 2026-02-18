using App.Domain.Enums;

namespace App.Application.Features.Receipts.Dtos;

/// <summary>
/// DTO FOR RECEIPT LIST ITEM
/// </summary>
public record ReceiptListDto(
    
    string Id,
    string? OrganizationName,
    decimal TotalAmount,
    Currency Currency,
    bool IsFavorite,
    SourceType Type,
    string? ImageUrl,
    DateTime CreatedAt,
    int ItemCount

    );

/// <summary>
/// DTO FOR RECEIPT DETAIL VIEW
/// </summary>
public record ReceiptDetailDto(
    
    string Id,
    string UserId,
    decimal TotalAmount,
    Currency Currency,
    bool IsFavorite,
    SourceType Type,
    string? ImageUrl,
    DateTime CreatedAt,
    ReceiptOrganizationDto? Organization,
    List<ReceiptItemDto> Items,
    string? PreviousReceiptId,
    string? NextReceiptId

    );

/// <summary>
/// DTO FOR ORGANIZATION INFO WITHIN RECEIPT
/// </summary>
public record ReceiptOrganizationDto(
    
    int Id,
    string Name,
    string? Branch,
    string? LogoUrl,
    string CountryCode,
    string City,
    string? Region

    );

/// <summary>
/// DTO FOR RECEIPT ITEM
/// </summary>
public record ReceiptItemDto(
    
    int Id,
    string Name,
    decimal Quantity,
    decimal TaxRate,
    decimal TaxAmount,
    decimal TotalPrice,
    decimal UnitPrice,
    string? UnitType,
    decimal? UnitSize,
    string? Category,
    string? Brand

    );


/// <summary>
/// RESPONSE DTO FOR RECEIPT CREATION
/// </summary>
public record CreateReceiptResponseDto(
    
    string Id,
    decimal TotalAmount,
    Currency Currency,
    SourceType Type,
    int ItemCount,
    DateTime CreatedAt

    );

/// <summary>
/// RESPONSE DTO FOR TOGGLE FAVORITE
/// </summary>
public record ToggleFavoriteResponseDto(
    
    string ReceiptId,
    bool IsFavorite

    );