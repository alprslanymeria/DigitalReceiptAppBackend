using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Receipts.CacheKeys;
using App.Application.Features.Receipts.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.Receipts.Commands.ToggleFavoriteCommand;

/// <summary>
/// HANDLER FOR TOGGLING RECEIPT FAVORITE STATUS.
/// VALIDATES OWNERSHIP AND INVALIDATES CACHE.
/// </summary>
public class ToggleFavoriteHandler(

    IReceiptRepository receiptRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<ToggleFavoriteCommand, ServiceResult<ToggleFavoriteResponseDto>>
{
    public async Task<ServiceResult<ToggleFavoriteResponseDto>> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetByIdAsync(request.ReceiptId);

        // GUARD: RECEIPT EXISTS
        if (receipt is null)
            throw new NotFoundException($"RECEIPT WITH ID '{request.ReceiptId}' WAS NOT FOUND");

        // GUARD: USER OWNS THE RECEIPT
        if (receipt.UserId != request.UserId)
            throw new BusinessException("YOU ARE NOT AUTHORIZED TO MODIFY THIS RECEIPT");

        // TOGGLE FAVORITE
        receipt.IsFavorite = !receipt.IsFavorite;
        receiptRepository.Update(receipt);
        await unitOfWork.CommitAsync();

        // INVALIDATE CACHE
        await cacheManager.RemoveByPrefixAsync(ReceiptCacheKeys.Prefix);

        var response = new ToggleFavoriteResponseDto(

            ReceiptId: receipt.Id,
            IsFavorite: receipt.IsFavorite

            );

        return ServiceResult<ToggleFavoriteResponseDto>.Success(response);
    }
}