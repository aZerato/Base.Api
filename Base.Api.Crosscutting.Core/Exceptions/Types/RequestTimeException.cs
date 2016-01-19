namespace Base.Api.Crosscutting.Core.Exceptions.Types
{
    using System;

    /// <summary>
    /// The RequestTime exception class.
    /// </summary>
    public class RequestTimeException : Exception, IApiException
    {
        /// <summary>
        /// Initializes a new instance of the RequestTimeException class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public RequestTimeException(string message)
            : base(message)
        { }
    }
}
