namespace SAC.Stock.Service.BuyContext
{
    using Seed.NLayer.ExceptionHandling;
    using Seed.NLayer.Data;
    using System.Collections.Generic;
    using System.ServiceModel; 

    [ServiceContract]
    internal interface IBuyApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BuyDto AddBuy(BuyDto buyInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BuyDto ModifyBuy(BuyDto buyInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BuyDto> QueryBuyNotStocked();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BuyDto> QueryBuyStocked();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BuyDto> QueryBuyNotBoxed();

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BuyDto> QueryBuyBoxed();
      
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BuyDto> QueryBuy(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);
    }
}