namespace Mabusall.Core.Behaviors;

public class FeatureBehavior<TRequest, TResponse>
    (IAppSettingsKeyManagement appSettingsKeyManagement, IFeatureManager featureManager)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        var featureName = typeof(TRequest).Name;

        if (appSettingsKeyManagement.AppFeatures.TryGetValue(featureName, out _))
        {
            // The feature is found, use the value of 'feature'
            if (!await featureManager.IsEnabledAsync(featureName))
                throw new FeatureDisabledException($"The feature {featureName} is not available for now.");
        }

        return await next();
    }
}