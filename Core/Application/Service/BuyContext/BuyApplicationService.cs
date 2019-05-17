namespace SAC.Stock.Service.BuyContext
{
    using Seed.NLayer.Data;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using Domain.BuyContext;    
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;    
    internal class BuyApplicationService : IBuyApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public BuyDto AddBuy(BuyDto buyInfo)
        {
            var svc = new BuyService(StockCtx);
            return svc.AddBuy(buyInfo).AdaptToBuyDto();
        }

        public BuyDto ModifyBuy(BuyDto buyInfo)
        {
            var svc = new BuyService(StockCtx);
            return svc.ModifyBuy(buyInfo).AdaptToBuyDto();
        }

        public ICollection<BuyDto> QueryBuyNotStocked()
        {
            var svc = new BuyService(StockCtx);
            return svc.QueryBuyNotStocked().Select(b => b.AdaptToBuyDto()).ToList();
        }

        public ICollection<BuyDto> QueryBuyStocked()
        {
            var svc = new BuyService(StockCtx);
            return svc.QueryBuyStocked().Select(b => b.AdaptToBuyDto()).ToList();
        }

        public ICollection<BuyDto> QueryBuyNotBoxed()
        {
            var svc = new BuyService(StockCtx);
            return svc.QueryBuyNotBoxed().Select(b => b.AdaptToBuyDto()).ToList();
        }

        public ICollection<BuyDto> QueryBuyBoxed()
        {
            var svc = new BuyService(StockCtx);
            return svc.QueryBuyBoxed().Select(b => b.AdaptToBuyDto()).ToList();
        }

        public ICollection<BuyDto> QueryBuy(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new BuyService(StockCtx);
                return svc.QueryBuy(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToBuyDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }
    }
}
