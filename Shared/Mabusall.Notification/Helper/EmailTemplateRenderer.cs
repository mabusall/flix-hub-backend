namespace Mabusall.Notification.Helper;

public class EmailTemplateRenderer(IRazorViewEngine engine,
                                   IServiceProvider serviceProvider,
                                   ITempDataProvider tempDataProvider)
    : IEmailTemplateRenderer
{
    public async Task<string> RenderRazorTemplateAsStringAsync<T>(string viewName, T model)
    {
        var httpContext = new DefaultHttpContext() { RequestServices = serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using StringWriter sw = new();
        var viewResult = engine.FindView(actionContext, viewName, false);

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

    public async Task<string> RenderRazorTemplateFromPathAsStringAsync<T>(string viewPath, T model)
    {
        var httpContext = new DefaultHttpContext() { RequestServices = serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using StringWriter sw = new();
        var viewResult = engine.GetView(viewPath, viewPath, false);

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