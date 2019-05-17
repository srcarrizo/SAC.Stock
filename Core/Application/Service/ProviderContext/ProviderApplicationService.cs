namespace SAC.Stock.Service.ProviderContext
{
    using SAC.Membership.Service.UserManagement;
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.ProviderContext;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    internal class ProviderApplicationService : IProviderApplicationService
    {
        public IDataContext StockCtx { get; set; }
                
        public ProviderDto AddProvider(ProviderDto providerInfo)
        {
            var svc = new ProviderService(StockCtx);
            return svc.AddProvider(providerInfo).AdaptToProviderDto();
        }

        public ProviderDto ModifyProvider(ProviderDto ProviderInfo)
        {
            var svc = new ProviderService(StockCtx);
            return svc.ModifyProvider(ProviderInfo).AdaptToProviderDto();
        }

        public ProviderDto GetProvider(Guid providerId)
        {
            var svc = new ProviderService(StockCtx);
            return svc.GetProvider(providerId).AdaptToProviderDto();
        }

        public ProviderDto GetProvider(string uidCode, string uidSerie)
        {
            var svc = new ProviderService(StockCtx);
            return svc.GetProvider(uidCode, uidSerie).AdaptToProviderDto();
        }

        public ICollection<ProviderDto> QueryProvider(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new ProviderService(StockCtx);
                return svc.QueryProvider(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToProviderDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }
    }
}
