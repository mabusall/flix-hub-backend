namespace FlixHub.Notification.Helper;

public class EmailTemplateRenderer(IRazorViewEngine engine,
                                   IServiceProvider serviceProvider,
                                   ITempDataProvider tempDataProvider)
    : IEmailTemplateRenderer
{
    public async Task<string> RenderRazorTemplateAsStringAsync<T>(string razorViewName, T model)
    {
        var httpContext = new DefaultHttpContext() { RequestServices = serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using StringWriter sw = new();
        var viewResult = engine.FindView(actionContext, razorViewName, false);

        if (!viewResult.Success) return string.Empty;

        var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                                                        new ModelStateDictionary())
        {
            Model = model
        };

        var viewContext = new ViewContext(actionContext,
                                          viewResult.View,
                                          viewDataDictionary,
                                          new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                                          sw,
                                          new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);

        return sw.ToString();
    }

    public async Task<string> RenderRazorTemplateFromPathAsStringAsync<T>(string razorViewPath, T model)
    {
        var httpContext = new DefaultHttpContext() { RequestServices = serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using StringWriter sw = new();
        var viewResult = engine.GetView(razorViewPath, razorViewPath, false);

        if (!viewResult.Success) return string.Empty;

        var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                                                        new ModelStateDictionary())
        {
            Model = model
        };

        var viewContext = new ViewContext(actionContext,
                                          viewResult.View,
                                          viewDataDictionary,
                                          new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                                          sw,
                                          new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);

        return sw.ToString();
    }
}