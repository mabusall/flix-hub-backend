namespace FlixHub.Core.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger,
                                                  ICurrentUserService currentUserService,
                                                  ICorrelationIdService correlationIdService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        string userId = currentUserService.UserId ?? string.Empty;
        string correlationId = correlationIdService.Get() ?? string.Empty;

        logger.LogInformation("[START] Handle request={Request} - RequestData={@RequestData} [UserId: {UserId}, CorrelationId: {CorrelationId}] - Response={Response}",
                              typeof(TRequest).Name,
                              request,
                              userId,
                              correlationId,
                              typeof(TResponse).Name);

        var timer = new Stopwatch();

        timer.Start();
        var response = await next(cancellationToken);
        timer.Stop();

        // if the request is greater than 3 seconds, then log the warnings
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                              typeof(TRequest).Name,
                              timeTaken.Seconds);

        logger.LogInformation("[END] Handled {Request} with {Response} - ResponseData={@ResponseData}",
                              typeof(TRequest).Name,
                              typeof(TResponse).Name,
                              response);
        return response;
    }
}
