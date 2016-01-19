namespace Base.Api.Crosscutting.Core.Exceptions.Types
{
    using System;
    using System.Net;

    /// <summary>
    /// The Response Status Code exception class.
    /// </summary>
    public class ResponseStatusCodeException : Exception, IApiException
    {
        #region properties

        /// <summary>
        /// The status code need to be reveled by exception.
        /// </summary>
        public HttpStatusCode statusCode { get; set; }

        #endregion properties

        #region constructors

        /// <summary>
        /// Initializes a new instance of the ResponseStatusCodeException class.
        /// </summary>
        /// <param name="statusCode">The status code to return.</param>
        /// <param name="message">The exception message.</param>
        public ResponseStatusCodeException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.statusCode = statusCode;
        }

        #endregion constructors
    }
}

