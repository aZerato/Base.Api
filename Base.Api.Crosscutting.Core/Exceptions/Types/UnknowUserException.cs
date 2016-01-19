namespace Base.Api.Crosscutting.Core.Exceptions.Types
{
    using System;

    /// <summary>
    /// The UnknowUser exception class.
    /// </summary>
    public class UnknowUserException : Exception, IApiException
    {
        /// <summary>
        /// Initializes a new instance of the UnknowUserException class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UnknowUserException(string message)
            : base(message)
        { }
    }
}

