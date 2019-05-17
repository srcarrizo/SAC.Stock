namespace SAC.Stock.Front.App_Start
{
    using SAC.Seed.Dependency;
    using SAC.Seed.Dependency.Unity;
    using SAC.Seed.Logging;
    using SAC.Seed.Validator;
    internal static class ContainerConfig
    {
        private static readonly object ThisLock = new object();
        private static bool initialized;
        public static void Initialize()
        {
            if (initialized)
            {
                return;
            }

            lock (ThisLock)
            {
                DiContainerFactory.SetProvider(new UnityDiContainerProvider());
                var container = DiContainerFactory.DiContainer();

                LoggerFactory.DefaultLoggerName = "Stock.WebSiteStock";
                LoggerFactory.SetProvider(container.Resolve<ILoggerProvider>());

                EntityValidatorFactory.SetProvider(container.Resolve<IEntityValidatorProvider>());

                initialized = true;
            }
        }
    }
}