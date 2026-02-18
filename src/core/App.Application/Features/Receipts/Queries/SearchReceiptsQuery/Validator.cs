using FluentValidation;

namespace App.Application.Features.Receipts.Queries.SearchReceiptsQuery;

/// <summary>
/// VALIDATOR FOR SEARCH RECEIPTS QUERY
/// </summary>
public class SearchReceiptsQueryValidator : AbstractValidator<SearchReceiptsQuery>
{
    public SearchReceiptsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .WithMessage("SEARCH TERM IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("SEARCH TERM MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("PAGE MUST BE GREATER THAN 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PAGE SIZE MUST BE BETWEEN 1 AND 100");
    }
}