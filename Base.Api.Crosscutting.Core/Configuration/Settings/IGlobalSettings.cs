namespace Base.Api.Crosscutting.Core.Configuration
{
    public interface IGlobalSettings
    {
        /// <summary>
        ///  Gets the WebServices name.
        /// </summary>
        string WebServicesName { get; }

        /// <summary>
        /// Gets the PrefixHeader key.
        /// </summary>
        string PrefixHeaderKey { get; }

        /// <summary>
        /// Gets the Request time validity.
        /// </summary>
        string RequestTimeValidity { get; }

        /// <summary>
        /// Gets the Max results return value.
        /// </summary>
        string MaxResultsReturn { get; }
    }
}
