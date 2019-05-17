namespace SAC.Stock.Service.SaleContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.SaleContext;    
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;    
    internal class SaleApplicationService : ISaleApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public SaleDto AddSale(SaleDto SaleInfo)
        {
            var svc = new SaleService(StockCtx);
            return svc.AddSale(SaleInfo).AdaptToSaleDto();
        }

        public SaleDto ModifySale(SaleDto SaleInfo)
        {
            var svc = new SaleService(StockCtx);
            return svc.ModifySale(SaleInfo).AdaptToSaleDto();
        }

        public ICollection<SaleDto> QuerySaleNotBoxed()
        {
            var svc = new SaleService(StockCtx);
            return svc.QuerySaleNotBoxed().Select(s => s.AdaptToSaleDto()).ToList();
        }

        public ICollection<SaleDto> QuerySaleNotStocked()
        {
            var svc = new SaleService(StockCtx);
            return svc.QuerySaleNotStocked().Select(s => s.AdaptToSaleDto()).ToList();
        }

        public ICollection<SaleDto> QuerySaleBoxed()
        {
            var svc = new SaleService(StockCtx);
            return svc.QuerySaleBoxed().Select(s => s.AdaptToSaleDto()).ToList();
        }

        public ICollection<SaleDto> QuerySaleStocked()
        {
            var svc = new SaleService(StockCtx);
            return svc.QuerySaleStocked().Select(s => s.AdaptToSaleDto()).ToList();
        }

        public SaleDto CompleteSale(SaleDto saleInfo)
        {
            var svc = new SaleService(StockCtx);
            return svc.CompleteSale(saleInfo).AdaptToSaleDto();
        }

        public bool DeletePreSale(SaleDto saleInfo)
        {
            var svc = new SaleService(StockCtx);
            return svc.DeletePreSale(saleInfo);
        }

        public ICollection<SaleDto> QuerySale(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new SaleService(StockCtx);
                return svc.QuerySale(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToSaleDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }
    }
}
