namespace SAC.Stock.Service.StockContext
{
    using SAC.Seed.NLayer.ExceptionHandling;
    using System;    
    using System.ServiceModel;    

    [ServiceContract]
    internal interface IStockApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        int GetStockByProduct(int productId);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        int CheckStockByProduct(int productId);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        StockDto CreateStock();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        StockDto GetStockByDate(DateTimeOffset stockDate);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        StockDto GetCurrentStock();        
    }
}