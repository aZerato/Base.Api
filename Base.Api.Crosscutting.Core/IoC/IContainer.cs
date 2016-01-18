namespace Base.Api.Crosscutting.Core.IoC
{
    using System;

    /// <summary>
    /// Contract for Ioc resolver.
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Solve TService dependency.
        /// </summary>
        /// <typeparam name="TService">Type of the needed object instance.</typeparam>
        /// <returns>Instance of TService.</returns>
        TService Resolve<TService>();

        /// <summary>
        /// Solve type construction and return the object as a TService instance.
        /// </summary>
        /// <param name="type">Type of the needed object instance.</param>
        /// <returns>Instance of this type.</returns>
        object Resolve(Type type);
    }
}
