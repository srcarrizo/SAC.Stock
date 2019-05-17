namespace SAC.Stock.Domain.BudgetContext
{
    using System.Linq;
    using BranchOfficeContext;
    using CustomerContext;
    using ProductContext;
    using Service.BudgetContext;
    internal static class BudgetAdapter
    {
        public static BudgetDto AdaptToBudgetDto(this Budget entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BudgetDto
            {
                Id = entity.Id,
                BudgetDate = entity.BudgetDate,
                Customer = entity.Customer?.AdaptToCustomerDto(),
                NonCustomerName = entity.NonCustomerName,
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = entity.BranchOfficeStaff.AdaptToFranchiseStaffDto(),
                PaymentTypeCode = entity.PaymentTypeCode,
                Detail = entity.Detail.Select(d => d.AdaptBudgetDetailToDto()).ToList(),
                CustomerId = entity.CustomerId,
                BranchOfficeStaffId = entity.BranchOfficeStaffId,
                BranchOfficeId = entity.BranchOfficeId
            };
        }

        public static BudgetDetailDto AdaptBudgetDetailToDto(this BudgetDetail entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BudgetDetailDto
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Price = entity.Price,
                Product = entity.Product.AdaptToProductDto()
            };
        }
    }
}
