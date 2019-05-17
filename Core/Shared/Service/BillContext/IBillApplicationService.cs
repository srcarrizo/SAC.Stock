namespace SAC.Stock.Service.BillContext
{
    using System.ServiceModel;
    using Seed.NLayer.ExceptionHandling;
    using BaseDto;
    using System.Collections.Generic;
    using SAC.Seed.NLayer.Data;

    [ServiceContract]
    internal interface IBillApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BillDto AddBill(BillDto billDto);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BillUnitTypeDto GetBillUnitType(int billUnitTypeId);
       
        [OperationContract]
        ICollection<BillUnitTypeDto> QueryBillUnitType(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);

        [OperationContract]
        ICollection<BillDto> QueryBill();
    }
}
