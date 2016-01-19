namespace Base.Api.Models
{
    /// <summary>
    /// The Error model.
    /// </summary>
    public class ApiErrorModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this error was expected or not.
        /// </summary>
        public bool WasExpected { get; set; }

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}