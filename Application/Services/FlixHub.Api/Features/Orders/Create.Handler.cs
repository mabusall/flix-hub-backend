namespace Store.Api.Features.Orders;

#region [ custom FluentValidation rule]

//internal record CustomValidationQuery(OrderType OrderType) : IQuery<bool>;

//internal class CustomValidationQueryHandler
//    (IStoreUnitOfWork storeSession)
//    : IQueryHandler<CustomValidationQuery, bool>
//{
//    public async Task<bool> Handle(CustomValidationQuery query, CancellationToken cancellationToken)
//    {
//        // query database
//        return await storeSession
//            .OrdersRepository
//            .AnyAsync(a => a.OrderType == query.OrderType, cancellationToken);
//    }
//}

#endregion

internal class CreateOrderCommandHandler
    (IStoreUnitOfWork storeSession, IBusService busService)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = command.Adapt<Order>();

        // add record to repository collection
        storeSession.OrdersRepository.Insert(order);

        // send email after success
        await busService.Publish(new EmailNotificationEvent
        {
            Subject = EmailNotificationResources.VerifyEmailTemplate_VerifyEmailTitle,
            LanguageIsoCode = "en",
            Priority = EmailPriority.High,
            Template = NotificationTemplate.VerifyOTP,
            ToAddresses = ["mabusell@tasheer.com"],
            ExtraData = new
            {
                OtpValue = "123456",
                SiteUrl = "www.google.com"
            },
        }, cancellationToken);

        // commit db changes
        await storeSession.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateOrderResult(order.Uuid);
    }
}