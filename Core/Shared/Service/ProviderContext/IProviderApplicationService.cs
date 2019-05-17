namespace SAC.Stock.Service.ProviderContext
{
    using Seed.NLayer.Data;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    internal interface IProviderApplicationService
    {
        [OperationContract]
        ProviderDto AddProvider(ProviderDto providerInfo);
        [OperationContract]
        ProviderDto ModifyProvider(ProviderDto ProviderInfo);
        [OperationContract]
        ProviderDto GetProvider(Guid providerId);
        [OperationContract]
        ProviderDto GetProvider(string uidCode, string uidSerie);
        [OperationContract]
        ICollection<ProviderDto> QueryProvider(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);
    }
}
