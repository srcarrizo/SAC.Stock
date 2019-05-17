using System;
using System.Collections.Generic;
using SAC.Seed.NLayer.Data.Ordering;

namespace SAC.Stock.Domain.BudgetContext
{
    using Code;    
    using Service.BudgetContext;
    using Seed.NLayer.Domain;    
    using Seed.NLayer.Data;
    using System.Linq;
    using SAC.Seed.NLayer.Domain.Specification;

    internal class BudgetService
    {
        private readonly IDataContext context;
        internal BudgetService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Budget, int> ViewBudget
        {
            get
            {
                return context.GetView<Budget, int>();
            }
        }

        private IDataView<BudgetDetail, int> ViewBudgetDetail
        {
            get
            {
                return context.GetView<BudgetDetail, int>();
            }
        }

        public Budget GetBudget(int budgetId)
        {
            return ViewBudget.Get(budgetId);
        }

        public Budget AddBudget(BudgetDto budgetInfo)
        {
            var budget = NewBudget(budgetInfo);
            context.ApplyChanges();
            return budget;
        }

        private Budget NewBudget(BudgetDto budgetInfo)
        {
            if (budgetInfo.BranchOffice == null)
            {
                throw BusinessRulesCode.BudgetWithoutBranchOffice.NewBusinessException();
            }

            if (budgetInfo.BranchOfficeStaff == null)
            {
                throw BusinessRulesCode.BudgetWithoutBranchOfficeStaff.NewBusinessException();
            }

            if ((budgetInfo.Customer == null) && string.IsNullOrEmpty(budgetInfo.NonCustomerName))
            {
                throw BusinessRulesCode.BudgetWithoutCustomer.NewBusinessException();
            }

            var budget = new Budget
            {
                BranchOfficeId = budgetInfo.BranchOffice.Id,
                BudgetDate = budgetInfo.BudgetDate,
                BranchOfficeStaffId = budgetInfo.BranchOfficeStaff.Id,
                PaymentTypeCode = budgetInfo.PaymentTypeCode,                
                Detail = budgetInfo.Detail.Select(d => new BudgetDetail
                {
                    Amount = d.Amount,
                    Price = d.Price,
                    ProductId = d.Product.Id
                }).ToList(),
                CustomerId = budgetInfo.Customer?.Id,
                NonCustomerName = budgetInfo.NonCustomerName
            };

            ViewBudget.Add(budget);

            return budget;
        }

        public IEnumerable<Budget> QueryBudget(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Budget>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Budget.BudgetDate, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Budget, DateTimeOffset>(b => b.BudgetDate, info.Direction));
                }
            }

            return ViewBudget.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetBudgetSpecification(filterInfo));
        }

        private static Specification<Budget> GetBudgetSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<Budget> spec = new TrueSpecification<Budget>();
            if (filterInfo == null)
            {
                return spec;
            }

            foreach (var info in filterInfo)
            {
                if (info.Spec.Equals(SpecFilter.Budget.BudgetDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBudgetByBudgetDate(info.Value);
                }
            }

            return spec;
        }

        private static Specification<Budget> SpecBudgetByBudgetDate(string value)
        {
            return new DirectSpecification<Budget>(s => s.BudgetDate.Equals(DateTimeOffset.Parse(value)));
        }
    }
}