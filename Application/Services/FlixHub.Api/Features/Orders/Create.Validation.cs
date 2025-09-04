namespace Store.Api.Features.Orders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(/*ISender sender*/)
    {
        RuleFor(r => r.Price)
            .GreaterThan(0)
            .WithMessage(ErrorMessageResources.ValueMustBeGreaterThanZero);

        RuleFor(r => r.Vat)
            .GreaterThan(0)
            .WithMessage(ErrorMessageResources.ValueMustBeGreaterThanZero);

        RuleFor(r => r.TotalPrice)
            .GreaterThan(0)
            .WithMessage(ErrorMessageResources.ValueMustBeGreaterThanZero);

        RuleFor(r => r.OrderType)
            .IsInEnum()
            .WithMessage(ErrorMessageResources.EnumIsNotDefined);

        RuleFor(r => r.Items)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotNull);

        // custom rule to access database
        //RuleFor(r => r.OrderType)
        //    .MustAsync(async (command, type, context, cancellation) =>
        //         !await sender.Send(new CustomValidationQuery(type), cancellation))
        //    .WithMessage(ErrorMessageResources.NotEmpty);
    }
}