namespace Base.Api.Crosscutting.Core.Exceptions.Types
{
    using System;

    /// <summary>
    /// The RequestParameter exception class.
    /// </summary>
    public class RequestParameterException : Exception, IApiException
    {
        /// <summary>
        /// Initializes a new instance of the RequestParameterException class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public RequestParameterException(string message)
            : base(message)
        { }
    }
}