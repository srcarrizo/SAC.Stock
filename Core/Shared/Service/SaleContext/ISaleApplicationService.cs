namespace SAC.Stock.Service.SaleContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;    
    using System.Collections.Generic;    
    using System.ServiceModel;    

    [ServiceContract]
    internal interface ISaleApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        SaleDto AddSale(SaleDto SaleInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        SaleDto ModifySale(SaleDto SaleInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<SaleDto> QuerySaleNotBoxed();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<SaleDto> QuerySaleNotStocked();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<SaleDto> QuerySaleBoxed();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<SaleDto> QuerySaleStocked();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        SaleDto CompleteSale(SaleDto saleInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        bool DeletePreSale(SaleDto saleInfo);
        
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<SaleDto> QuerySale(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);
    }
}