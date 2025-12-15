using System.Net;
using FluentValidation;
using MediatR;

namespace App.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            var errors = failures.Select(x => x.ErrorMessage).ToList();

            // If response type is ServiceResult or ServiceResult<T>, return error result
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(ServiceResult<>))
            {
                var resultType = typeof(ServiceResult<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
                var failMethod = resultType.GetMethod("Fail", new[] { typeof(List<string>), typeof(HttpStatusCode) });
                return (TResponse)failMethod!.Invoke(null, new object[] { errors, HttpStatusCode.BadRequest })!;
            }
            else if (typeof(TResponse) == typeof(ServiceResult))
            {
                return (TResponse)(object)ServiceResult.Fail(errors, HttpStatusCode.BadRequest);
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}
