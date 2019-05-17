using System.Linq;
using SAC.Stock.Front.Models.BranchOffice;
using SAC.Stock.Front.Models.Customer;
using SAC.Stock.Front.Models.Product;
using SAC.Stock.Service.BudgetContext;

namespace SAC.Stock.Front.Models.Budget
{
    internal static class BudgetDtoAdapter
    {
        public static BudgetDto AdaptToBudgetDto(this BudgetDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new BudgetDto
            {
                Id = dpo.Id,
                Customer = dpo.Customer?.AdaptToCustomerDto(),
                BudgetDate = dpo.BudgetDate,
                NonCustomerName = dpo.NonCustomerName,
                BranchOffice = dpo.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = dpo.BranchOfficeStaff.AdaptToBranchOfficeStaffSaveDto(),
                PaymentTypeCode = dpo.PaymentTypeCode,
                Detail = dpo.Detail.Select(d => d.AdaptToBudgetDetailDto()).ToList(),
                CustomerId = dpo.CustomerId,
                BranchOfficeStaffId = dpo.BranchOfficeStaffId,
                BranchOfficeId = dpo.BranchOfficeId
            };
        }

        public static BudgetDetailDto AdaptToBudgetDetailDto(this BudgetDetailDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new BudgetDetailDto
            {
                Id = dpo.Id,
                Amount = dpo.Amount,
                Price = dpo.Price,
                Product = dpo.Product.AdaptToProductDto(),
                ProductId = dpo.Product.Id
            };
        }
    }
}