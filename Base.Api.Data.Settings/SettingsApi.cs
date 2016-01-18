namespace Base.Api.Data.Settings
{
    using Crosscutting.Core.Configuration;
    using System.Configuration;
    /// <summary>
    /// SettingsApi implementation.
    /// </summary>
    public class SettingsApi : ISettingsApi
    {
        /// <summary>
        /// Tag section name in configuration file.
        /// </summary>
        private const string SettingSection = "baseApiConfiguration";

        /// <summary>
        /// Get the configuration Api section.
        /// </summary>
        public IApplicationSettings ApplicationSettings { get { return ConfigurationManager.GetSection(SettingSection) as ApplicationSettings; } }
    }
}
