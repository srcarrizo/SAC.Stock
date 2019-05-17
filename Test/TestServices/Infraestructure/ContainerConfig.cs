namespace SAC.Stock.Test.TestServices.Infraestructure
{
    using SAC.Seed.Dependency;
    using SAC.Seed.Dependency.Unity;
    using SAC.Seed.Logging;
    using SAC.Seed.Serialize;
    using SAC.Seed.Validator;
    public static class ContainerConfig
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

                LoggerFactory.SetProvider(container.Resolve<ILoggerProvider>());

                EntityValidatorFactory.SetProvider(container.Resolve<IEntityValidatorProvider>());

                SerializerFactory.SetProvider(container.Resolve<ISerializerProvider>());

                initialized = true;
            }
        }

    }
}