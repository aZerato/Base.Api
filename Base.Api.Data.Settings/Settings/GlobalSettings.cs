namespace Base.Api.Data.Settings
{
    using Crosscutting.Core.Configuration;
    using System.Configuration;

    /// <summary>
    /// GlobalSettings class.
    /// </summary>
    internal class GlobalSettings : ConfigurationElement, IGlobalSettings
    {
        #region properties

        /// <summary>
        /// WebServices name property name.
        /// </summary>
        private const string WebServicesNamePropertyName = "webServicesName";

        /// <summary>
        /// WebServices name property name.
        /// </summary>
        private const string PrefixHeaderKeyPropertyName = "prefixHeaderKey";

        /// <summary>
        /// Request time validity property name.
        /// </summary>
        private const string RequestTimeValidityPropertyName = "requestTimeValidity";

        /// <summary>
        /// Max results return property name.
        /// </summary>
        private const string MaxResultsReturnPropertyName = "maxResultsReturn";

        #endregion properties

        #region attributes

        /// <summary>
        /// Gets the WebServices name.
        /// </summary>
        [ConfigurationProperty(WebServicesNamePropertyName)]
        public string WebServicesName { get { return this[WebServicesNamePropertyName] as string; } }

        /// <summary>
        /// Gets the PrefixHeader key.
        /// </summary>
        [ConfigurationProperty(PrefixHeaderKeyPropertyName)]
        public string PrefixHeaderKey { get { return this[PrefixHeaderKeyPropertyName] as string; } }

        /// <summary>
        /// Gets the Request time validity.
        /// </summary>
        [ConfigurationProperty(RequestTimeValidityPropertyName)]
        public string RequestTimeValidity { get { return this[RequestTimeValidityPropertyName] as string; } }

        /// <summary>
        /// Gets the Max results return.
        /// </summary>
        [ConfigurationProperty(MaxResultsReturnPropertyName)]
        public string MaxResultsReturn { get { return this[MaxResultsReturnPropertyName] as string; } }

        #endregion attributes
    }
}
