using FluentValidation;

namespace App.Application.Features.Receipts.Queries.GetReceiptsPagedQuery;

/// <summary>
/// VALIDATOR FOR GET RECEIPTS PAGED QUERY
/// </summary>
public class GetReceiptsPagedQueryValidator : AbstractValidator<GetReceiptsPagedQuery>
{
    public GetReceiptsPagedQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("PAGE MUST BE GREATER THAN 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PAGE SIZE MUST BE BETWEEN 1 AND 100");
    }
}