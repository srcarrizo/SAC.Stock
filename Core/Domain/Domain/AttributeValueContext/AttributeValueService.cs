using SAC.Seed.NLayer.Data;

namespace SAC.Stock.Domain.AttributeValueContext
{
    internal class AttributeValueService
    {
        private readonly IDataContext context;

        public AttributeValueService(IDataContext context)
        {
            this.context = context;
        }
    }
}