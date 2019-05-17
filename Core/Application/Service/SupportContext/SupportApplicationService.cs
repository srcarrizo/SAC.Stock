namespace SAC.Stock.Service.SupportContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Stock.Domain.PhoneContext;
    using SAC.Stock.Service.BaseDto;
    using System.Collections.Generic;
    using System.Linq;

    internal class SupportApplicationService : ISupportApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public TelcoDto GetTelco(int id)
        {
            var svc = new PhoneService(StockCtx);
            return svc.GetTelco(id).AdaptToTelcoDto();
        }

        public TelcoDto GetTelco(string code)
        {
            var svc = new PhoneService(StockCtx);
            return svc.GetTelco(code).AdaptToTelcoDto();
        }

        public List<TelcoDto> QueryTelco()
        {
            var svc = new PhoneService(StockCtx);
            return svc.QueryTelco().Select(t => t.AdaptToTelcoDto()).ToList();
        }
    }
}
