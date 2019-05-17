namespace SAC.Stock.Service.CustomerContext
{    
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.CustomerContext;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    internal class CustomerApplicationService : ICustomerApplicationService
    {
        public IDataContext StockCtx { get; set; }        

        public CustomerDto AddCustomer(CustomerDto customerInfo)
        {
            var svc = new CustomerService(StockCtx);
            return svc.AddCustomer(customerInfo).AdaptToCustomerDto();
        }

        public CustomerDto ModifyCustomer(CustomerDto customerInfo)
        {
            var svc = new CustomerService(StockCtx);
            return svc.ModifyCustomer(customerInfo).AdaptToCustomerDto();
        }


        public CustomerDto GetCustomer(Guid customerId)
        {
            var svc = new CustomerService(StockCtx);
            return svc.GetCustomer(customerId).AdaptToCustomerDto();
        }

        public CustomerDto GetCustomer(string uidCode, string uidSerie)
        {
            var svc = new CustomerService(StockCtx);
            return svc.GetCustomer(uidCode, uidSerie).AdaptToCustomerDto();
        }

        public ICollection<CustomerDto> QueryCustomer(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new CustomerService(StockCtx);
                return svc.QueryCustomer(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToCustomerDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }
    }
}
