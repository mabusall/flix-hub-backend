namespace Tasheer.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(validators.Select(s => s.ValidateAsync(validationContext, cancellationToken)));

        var failures =
            validationResults
            .Where(w => w.Errors.Count != 0)
            .SelectMany(vr => vr.Errors)
            .ToList();

        if (failures.Count != 0) throw new FluentValidation.ValidationException(failures);

        return await next();
    }
}