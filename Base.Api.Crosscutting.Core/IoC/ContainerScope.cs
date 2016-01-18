namespace Base.Api.Crosscutting.Core.IoC
{
    /// <summary>
    /// Enumeration representing the scope of the container.
    /// </summary>
    public enum ContainerScope
    {
        /// <summary>
        /// Root scope.
        /// </summary>
        Root = 0,

        /// <summary>
        /// RealApp scope.
        /// </summary>
        RealApp = 1,

        /// <summary>
        /// FakeApp scope.
        /// </summary>
        FakeApp = 2
    }
}
