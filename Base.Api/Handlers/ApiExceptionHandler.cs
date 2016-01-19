namespace Base.Api.Handlers
{
    using Crosscutting.Core.Exceptions;
    using Crosscutting.Core.Exceptions.Types;
    using Models;
    using System;
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// Api exception handler.
    /// </summary>
    public class ApiExceptionHandler
    {
        /// <summary>
        /// Handles exceptions and return the appropriate HTTP response message, with HTTP error code and message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public HttpResponseMessage HandleException(Exception exception, HttpRequestMessage request)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            ApiErrorModel error = new ApiErrorModel
            {
                WasExpected = false,
                Message = "An unexpected error occured.",
                ExceptionMessage = exception.Message,
                Type = exception.GetType().Name
            };

            // handle unauthorized case
            if (exception is UnauthorizedAccessException ||
                exception is UnknowUserException ||
                exception is RequestTimeException ||
                exception is RequestParameterException)
            {
                status = HttpStatusCode.Unauthorized;
                error.WasExpected = true;
                error.Message = "Authorization has been denied for this request";
            }
            else if (exception is ResponseStatusCodeException)
            {
                ResponseStatusCodeException rhEx = ((ResponseStatusCodeException)exception);
                status = rhEx.statusCode;
                error.WasExpected = true;
                error.Message = "An error occured";
            }
            // handle expected provider expcetions
            else if (exception is IApiException)
            {
                error.Message = "An error occured.";
                error.WasExpected = true;
            }

            return request.CreateResponse(status, error);
        }
    }
}