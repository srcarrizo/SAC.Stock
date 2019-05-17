namespace SAC.Stock.Service.BudgetContext
{
    using System.Data;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using Domain.BudgetContext;
    using Seed.NLayer.Data;
    using System.Collections.Generic;
    using System.Linq;

    internal class BudgetApplicationService : IBudgetApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public BudgetDto GetBudget(int budgetId)
        {
            var svc = new BudgetService(StockCtx);
            return svc.GetBudget(budgetId).AdaptToBudgetDto();
        }

        public BudgetDto AddBudget(BudgetDto budgetInfo)
        {
            var svc = new BudgetService(StockCtx);
            return svc.AddBudget(budgetInfo).AdaptToBudgetDto();
        }

        public ICollection<BudgetDto> QueryBudget(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new BudgetService(StockCtx);
                return svc.QueryBudget(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToBudgetDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }
    }
}