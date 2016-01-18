namespace Base.Api.Crosscutting.Core.Configuration
{
    /// <summary>
    /// ISettingsApi Interface.
    /// </summary>
    public interface ISettingsApi
    {
        /// <summary>
        /// Gets application settings.
        /// </summary>
        IApplicationSettings ApplicationSettings { get; }
    }
}
