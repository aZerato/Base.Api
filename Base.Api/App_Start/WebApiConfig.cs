namespace Base.Api
{
    using ContentNegotiators;
    using CustomAttributes;
    using Filters;
    using Handlers;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services API Web

            // Itinéraires de l'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ); 
            
            // Web API configuration and services
            config.Filters.Add(new CustomAuthorizationAttribute());
            
            ApiExceptionHandler exceptionHandler = new ApiExceptionHandler();
            // Add our custom exception filter.
            config.Filters.Add(new ApiExceptionFilterAttribute(exceptionHandler));

            // Disable circular reference check for JSON
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Force our own JSON content negociator
            // We could have cleared all formatters then add the JsonMediaTypeFormatter, BUT this will lead
            // to content negotiation for each request, which means a little overhead.
            // Using a custom content negotiator like below will avoid this overhead, since there won't be
            // any negotiation (cf http://www.strathweb.com/2013/06/supporting-only-json-in-asp-net-web-api-the-right-way/)
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(config.Formatters.JsonFormatter));
        }
    }
}
