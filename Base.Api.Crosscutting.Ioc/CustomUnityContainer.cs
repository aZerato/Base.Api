namespace Base.Api.Crosscutting.Ioc
{
    using Core.Configuration;
    using Data.Managers;
    using Data.Managers.Interfaces;
    using Data.Settings;
    using Interfaces;
    using Microsoft.Practices.Unity;
    using System;

    /// <summary>
    /// Custom IoC implementation.
    /// </summary>
    public class CustomUnityContainer : ICustomUnityContainer
    {
        #region properties

        /// <summary>
        /// Singleton instance member.
        /// </summary>
        private static readonly CustomUnityContainer instance;

        /// <summary>
        /// Container.
        /// </summary>
        private static IUnityContainer container;

        #endregion properties

        #region constructors

        /// <summary>
        /// Initializes static members of the IocUnityContainer class.
        /// </summary>
        static CustomUnityContainer()
        {
            container = new UnityContainer();
            instance = new CustomUnityContainer();
        }

        /// <summary>
        /// Prevents a default instance of the IocUnityContainer class from being created.
        /// </summary>
        private CustomUnityContainer()
        {
            ConfigureContainer(container);
        }

        #endregion constructors

        /// <summary>
        /// Gets singleton instance of IoCFactory.
        /// </summary>
        public static CustomUnityContainer Instance
        {
            get
            {
                return instance;
            }
        }

        #region publics

        /// <summary>
        /// Solve TItem dependency.
        /// </summary>
        /// <typeparam name="TItem">Type of dependency.</typeparam>
        /// <returns>Instance of TItem.</returns>
        public TItem Resolve<TItem>()
        {
            return container.Resolve<TItem>();
        }

        /// <summary>
        /// Solve type construction and return the object as a TService instance.
        /// </summary>
        /// <param name="type">Type of the needed object instance.</param>
        /// <returns>Instance of this type.</returns>
        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion publics

        #region privates

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Dispose managed resources.</param>
        private static void Dispose(bool disposing)
        {
            if (disposing)
            {
                container.Dispose();
            }
        }

        /// <summary>
        /// Configure container.Register types and life time managers for unity builder process.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        private static void ConfigureContainer(IUnityContainer container)
        {
            // Register crosscuting mappings
            container.RegisterType<ISettingsApi, SettingsApi>(new TransientLifetimeManager());

            // Register managers
            container.RegisterType<ICryptoAuthorizeManager, CryptoAuthorizeManager>(new TransientLifetimeManager());
        }
        #endregion privates
    }
}
