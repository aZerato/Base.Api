namespace Base.Api.Filters
{
    using Handlers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Custom exception filter attribute.
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// The exception handler.
        /// </summary>
        private ApiExceptionHandler exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the ApiExceptionFilterAttribute class.
        /// </summary>
        /// <param name="exceptionHandler"></param>
        public ApiExceptionFilterAttribute(ApiExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = this.exceptionHandler.HandleException(
                actionExecutedContext.Exception,
                actionExecutedContext.Request);
        }
    }
}