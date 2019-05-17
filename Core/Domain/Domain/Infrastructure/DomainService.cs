namespace SAC.Stock.Domain.Infrastructure
{
    using SAC.Seed.NLayer.Data;
    public abstract class DomainService
    {
        protected DomainService(IDataContext context)
        {
            this.Context = context;
        }

        public IDataContext Context { get; private set; }
    }
}
