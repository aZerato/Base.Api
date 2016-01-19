namespace Base.Api.ContentNegotiators
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;

    /// <summary>
    /// A content negociator, which always return JSON.
    /// </summary>
    public class JsonContentNegotiator : IContentNegotiator
    {
        /// <summary>
        /// Gets or sets the JSON formatter.
        /// </summary>
        public JsonMediaTypeFormatter JsonFormatter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the JsonContentNegotiator class.
        /// </summary>
        /// <param name="jsonFormatter">The JSON formatter.</param>
        public JsonContentNegotiator(JsonMediaTypeFormatter jsonFormatter)
        {
            this.JsonFormatter = jsonFormatter;
        }

        /// <summary>
        /// Performs content negotiating by selecting the most appropriate System.Net.Http.Formatting.MediaTypeFormatter
        ///     out of the passed in formatters for the given request that can serialize
        ///     an object of the given type.
        /// </summary>
        /// <param name="type">The type to be serialized.</param>
        /// <param name="request">Request message, which contains the header values used to perform negotiation.</param>
        /// <param name="formatters">The set of System.Net.Http.Formatting.MediaTypeFormatter objects from which
        ///     to choose.</param>
        /// <returns>The result of the negotiation containing the most appropriate System.Net.Http.Formatting.MediaTypeFormatter
        ///     instance, or null if there is no appropriate formatter.</returns>
        public ContentNegotiationResult Negotiate(Type type, System.Net.Http.HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(this.JsonFormatter, new MediaTypeHeaderValue("application/json")); ;
        }
    }
}