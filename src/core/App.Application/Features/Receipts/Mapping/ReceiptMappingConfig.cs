using App.Application.Features.Receipts.Dtos;
using App.Domain.Entities;
using Mapster;

namespace App.Application.Features.Receipts.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR RECEIPT ENTITY MAPPINGS.
/// </summary>
public class ReceiptMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Receipt, ReceiptListDto>()
            .Map(dest => dest.OrganizationName, src => src.Organization != null ? src.Organization.Name : null)
            .Map(dest => dest.ItemCount, src => src.ReceiptItems.Count);

        config.NewConfig<Receipt, ReceiptDetailDto>()
            .Map(dest => dest.Organization, src => src.Organization)
            .Map(dest => dest.Items, src => src.ReceiptItems);

        config.NewConfig<Organization, ReceiptOrganizationDto>();

        config.NewConfig<ReceiptItem, ReceiptItemDto>();

        config.NewConfig<Receipt, CreateReceiptResponseDto>()
            .Map(dest => dest.ItemCount, src => src.ReceiptItems.Count);
    }
}
