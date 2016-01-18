namespace Base.Api.Data.Settings
{
    using Crosscutting.Core.Configuration;
    using System.Configuration;

    /// <summary>
    /// ApplicationSettings class.
    /// </summary>
    internal class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        #region properties

        /// <summary>
        /// Property name in application configuration for "global" settings.
        /// </summary>
        private const string GlobalPropertyName = "global";

        #endregion properties

        #region attributes

        /// <summary>
        /// Gets the "global" settings.
        /// </summary>
        public IGlobalSettings GlobalSettings { get { return this.Global; } }

        /// <summary>
        /// Global Configuration property.
        /// </summary>
        [ConfigurationProperty(GlobalPropertyName)]
        private GlobalSettings Global { get { return this[GlobalPropertyName] as GlobalSettings; } }

        #endregion attributes
    }
}

