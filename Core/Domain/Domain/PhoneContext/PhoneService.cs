namespace SAC.Stock.Domain.PhoneContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Domain;
    using SAC.Seed.NLayer.Domain.Specification;
    using SAC.Stock.Domain.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class PhoneService
    {
        private readonly IDataContext context;
        public PhoneService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Telco, int> ViewTelco
        {
            get
            {
                return context.GetView<Telco, int>();
            }
        }

        public Telco GetTelco(int id)
        {
            return ViewTelco.Get(id);
        }

        public Telco GetTelco(string code)
        {
            return ViewTelco.GetFirst(GetTelcoSpecByCode(code));
        }

        public ICollection<Telco> QueryTelco()
        {
            return ViewTelco.GetAll().ToArray();
        }

        private static Specification<Telco> GetTelcoSpecByCode(string code)
        {
            return
              new DirectSpecification<Telco>(
                c =>
                c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
