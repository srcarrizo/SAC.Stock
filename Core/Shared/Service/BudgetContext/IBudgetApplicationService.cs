namespace SAC.Stock.Service.BudgetContext
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Seed.NLayer.Data;
    using Seed.NLayer.ExceptionHandling;

    [ServiceContract]
    internal interface IBudgetApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BudgetDto GetBudget(int budgetId);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BudgetDto AddBudget(BudgetDto budgetInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BudgetDto> QueryBudget(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo,
            ICollection<FilterInfo> filterInfo = null);
    }
}
