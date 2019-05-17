namespace SAC.Stock.Service.CustomerContext
{
    using SAC.Seed.NLayer.Data;
    using System;
    using System.Collections.Generic;    
    using System.ServiceModel;    

    [ServiceContract]
    internal interface ICustomerApplicationService
    {
        [OperationContract]
        CustomerDto AddCustomer(CustomerDto customerInfo);
        [OperationContract]
        CustomerDto ModifyCustomer(CustomerDto customerInfo);
        [OperationContract]
        CustomerDto GetCustomer(Guid customerId);
        [OperationContract]
        CustomerDto GetCustomer(string uidCode, string uidSerie);
        [OperationContract]
        ICollection<CustomerDto> QueryCustomer(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);
    }
}
