using SAC.Stock.Front.Models.Bill;
using SAC.Stock.Front.Models.BranchOffice;
using SAC.Stock.Service.BillContext;
using SAC.Stock.Service.TransactionContext;
using System.Linq;

namespace SAC.Stock.Front.Models.Transaction
{
    internal static class TransactionDtoAdapter
    {       
        public static TransactionDpo AdaptToTransactionDpo(this TransactionDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new TransactionDpo
            {
                BoxId = dto.BoxId,
                BranchOfficeId = dto.BranchOfficeId,
                Id = dto.Id,
                Name = dto.Name,
                BranchOffice = dto.BranchOffice.AdaptToBranchOfficeDpo(),
                BranchOfficeStaffId = dto.BranchOfficeStaffId,
                BuyId = dto.BuyId,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivatedNote = dto.DeactivatedNote,
                SaleId = dto.SaleId,
                TransactionTypeInOut = dto.TransactionTypeInOut,
                TransactionDate = dto.TransactionDate,
                Detail = dto.Detail.Select(d => d.AdaptToTransactionDetailDpo()).ToList()                
            };
        }

        public static TransactionDetailDpo AdaptToTransactionDetailDpo(this TransactionDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new TransactionDetailDpo
            {
                Amount = dto.Amount,
                Id = dto.Id,
                Bill = new BillDpo
                {
                     BillUnitType = new BillUnitTypeDpo
                     {
                         Id = dto.Bill.BillUnitType.Id,
                         Code = dto.Bill.BillUnitType.Code,
                         Name = dto.Bill.BillUnitType.Name,
                         IsDecimal = dto.Bill.BillUnitType.IsDecimal
                     }, 
                     Code = dto.Bill.Code,
                     Id = dto.Bill.Id,
                     Name = dto.Bill.Name,
                     Value = dto.Bill.Value
                }
            };
        }

        public static TransactionDto AdaptToTransactionDto (this TransactionDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new TransactionDto
            {
                Id = dpo.Id,
                Name = dpo.Name,
                Description = dpo.Description,
                DeactivatedDate = dpo.DeactivatedDate,
                DeactivatedNote = dpo.DeactivatedNote,
                BoxId = dpo.BoxId,
                BuyId = dpo.BuyId,
                SaleId = dpo.SaleId,                   
                BranchOfficeId = dpo.BranchOffice.Id,
                BranchOfficeStaffId = dpo.BranchOfficeStaff.Id,
                TransactionDate = dpo.TransactionDate,
                TransactionTypeInOut = dpo.TransactionTypeInOut,
                Detail = dpo.Detail.Select(d => d.AdaptToTransactionDetailDto()).ToList()
            };
        }

        public static TransactionDetailDto AdaptToTransactionDetailDto(this TransactionDetailDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new TransactionDetailDto
            {
                
                Amount = dpo.Amount,
                Id = dpo.Id,                
                Bill = new BillDto
                {
                    Id = dpo.Bill.Id
                }
            };
        }
    }
}