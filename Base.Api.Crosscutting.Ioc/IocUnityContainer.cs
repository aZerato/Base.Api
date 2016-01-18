namespace Base.Api.Crosscutting.Ioc
{
    using Core.Configuration;
    using Core.IoC;
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    /// <summary>
    /// Implemented container in Microsoft Practices Unity.
    /// </summary>
    public sealed class IocUnityContainer : IContainer
    {
        #region properties

        /// <summary>
        /// Container configuration key.
        /// </summary>
        private const string DefaultIoCContainerConfigKey = "defaultIoCContainer";

        /// <summary>
        /// Singleton instance member.
        /// </summary>
        private static readonly IocUnityContainer instance;

        /// <summary>
        /// Containers dictionary.
        /// </summary>
        private static Dictionary<ContainerScope, IUnityContainer> containerScopes;

        /// <summary>
        /// Current container.
        /// </summary>
        private static ContainerScope currentScope;

        /// <summary>
        /// Settings provider lifetime manager.
        /// </summary>
        private static ContainerControlledLifetimeManager settingsProviderLifetimeManager;

        /// <summary>
        /// Logger lifetime manager.
        /// </summary>
        private static ContainerControlledLifetimeManager loggerLifetimeManager;

        #endregion properties

        #region constructors

        /// <summary>
        /// Initializes static members of the IocUnityContainer class.
        /// </summary>
        static IocUnityContainer()
        {
            containerScopes = new Dictionary<ContainerScope, IUnityContainer>();
            settingsProviderLifetimeManager = new ContainerControlledLifetimeManager();
            loggerLifetimeManager = new ContainerControlledLifetimeManager();
            instance = new IocUnityContainer();
        }

        /// <summary>
        /// Prevents a default instance of the IocUnityContainer class from being created.
        /// </summary>
        private IocUnityContainer()
        {
            ContainerScope configuredScope;

            // We use the default container specified in AppSettings
            if (!Enum.TryParse<ContainerScope>(ConfigurationManager.AppSettings[DefaultIoCContainerConfigKey], out configuredScope))
            {
                throw new ConfigurationErrorsException("defaultIoCContainer configuration key is not defined");
            }

            currentScope = configuredScope;

            // Create root container
            IUnityContainer rootContainer = new UnityContainer();
            containerScopes.Add(ContainerScope.Root, rootContainer);

            // Create container for real context, child of root container
            IUnityContainer realAppContainer = rootContainer.CreateChildContainer();
            containerScopes.Add(ContainerScope.RealApp, realAppContainer);

            // Create container for testing, child of root container
            IUnityContainer fakeAppContainer = rootContainer.CreateChildContainer();
            containerScopes.Add(ContainerScope.FakeApp, fakeAppContainer);

            ConfigureRootContainer(rootContainer);
            ConfigureRealContainer(realAppContainer);
            ConfigureFakeContainer(fakeAppContainer);
        }

        #endregion constructors

        /// <summary>
        /// Gets singleton instance of IoCFactory.
        /// </summary>
        public static IocUnityContainer Instance
        {
            get { return instance; }
        }

        #region publics

        /// <summary>
        /// Solve TItem dependency.
        /// </summary>
        /// <typeparam name="TItem">Type of dependency.</typeparam>
        /// <returns>Instance of TItem.</returns>
        public TItem Resolve<TItem>()
        {
            if (!containerScopes.ContainsKey(currentScope))
            {
                throw new ConfigurationErrorsException(string.Format("Current scope {0} is not defined!", currentScope.ToString()));
            }

            IUnityContainer container = containerScopes[currentScope];

            return container.Resolve<TItem>();
        }

        /// <summary>
        /// Solve type construction and return the object as a TService instance.
        /// </summary>
        /// <param name="type">Type of the needed object instance.</param>
        /// <returns>Instance of this type.</returns>
        public object Resolve(Type type)
        {
            if (!containerScopes.ContainsKey(currentScope))
            {
                throw new ConfigurationErrorsException(string.Format("Current scope {0} is not defined!", currentScope.ToString()));
            }

            IUnityContainer container = containerScopes[currentScope];

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
                foreach (IUnityContainer container in containerScopes.Values)
                {
                    container.Dispose();
                }

                loggerLifetimeManager.Dispose();
                settingsProviderLifetimeManager.Dispose();
            }
        }

        /// <summary>
        /// Configure real container. Register types and life time managers for unity builder process.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        private static void ConfigureRealContainer(IUnityContainer container)
        {
            //container.RegisterType<IReferentialDbContext, ReferentialDbContext>(new TransientLifetimeManager());
            //container.RegisterType<IGameDbContext, GameDbContext>(new TransientLifetimeManager());
        }

        /// <summary>
        /// Configure fake container.Register types and life time managers for unity builder process.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        private static void ConfigureFakeContainer(IUnityContainer container)
        {
            //container.RegisterType<IDbContext, FakeDataContext>(new TransientLifetimeManager());
        }

        /// <summary>
        /// Configure root container.Register types and life time managers for unity builder process.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        private static void ConfigureRootContainer(IUnityContainer container)
        {
            // Register crosscuting mappings
            container.RegisterType<ISettingsApi, Data.Settings.SettingsApi>(settingsProviderLifetimeManager);

        }
        #endregion privates
    }
}
