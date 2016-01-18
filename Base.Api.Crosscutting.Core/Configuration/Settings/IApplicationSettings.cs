namespace Base.Api.Crosscutting.Core.Configuration
{
    /// <summary>
    /// Interface Application settings contract.
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// Gets global settings.
        /// </summary>
        IGlobalSettings GlobalSettings { get; }
    }
}
