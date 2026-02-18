using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Domain.Exceptions;

namespace App.Application.Features.Receipts.Commands.DeleteReceiptCommand;

/// <summary>
/// HANDLER FOR DELETING A RECEIPT.
/// VALIDATES OWNERSHIP BEFORE DELETION.
/// </summary>
public class DeleteReceiptHandler(

    IReceiptRepository receiptRepository,
    IFileStorageHelper fileStorageHelper,
    IUnitOfWork unitOfWork
    
    ) : ICommandHandler<DeleteReceiptCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetByIdAsync(request.ReceiptId);

        // GUARD: RECEIPT EXISTS
        if (receipt is null)
            throw new NotFoundException($"RECEIPT WITH ID '{request.ReceiptId}' WAS NOT FOUND");

        // GUARD: USER OWNS THE RECEIPT
        if (receipt.UserId != request.UserId)
            throw new BusinessException("YOU ARE NOT AUTHORIZED TO DELETE THIS RECEIPT");

        // DELETE IMAGE FROM STORAGE IF EXISTS
        if (!string.IsNullOrWhiteSpace(receipt.ImageUrl))
        {
            await fileStorageHelper.DeleteFileFromStorageAsync(receipt.ImageUrl);
        }

        receiptRepository.Delete(receipt);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
    }
}

