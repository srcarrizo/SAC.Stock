namespace SAC.Stock.Domain.TransactionContext
{
    using Service.TransactionContext;
    using BranchOfficeContext;
    using BuyContext;
    using SaleContext;
    using BoxContext;
    using BillContext;
    using System.Linq;    

    internal static class TransactionAdapter
    {
        public static TransactionDto AdaptToTransactionDtoComplete(this Transaction entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TransactionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Box = entity.Box.AdaptBoxToDto(),
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = entity.BranchOfficeStaff.AdaptToFranchiseStaffDto(),
                Buy = entity.Buy.AdaptToBuyDto(),
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,
                Sale = entity.Sale.AdaptToSaleDto(),
                TransactionDate = entity.TransactionDate,
                TransactionTypeInOut = entity.TransactionTypeInOut,
                Detail = entity.Detail.Select(d => d.AdaptTransactionDetailToDtoComplete()).ToList()                
            };
        }

        public static TransactionDetailDto AdaptTransactionDetailToDtoComplete(this TransactionDetail entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TransactionDetailDto
            {
                Amount = entity.Amount,
                Id = entity.Id,
                Bill = entity.Bill.AdaptToBillDto()
            };
        }

        public static TransactionDto AdaptTransactionToDto(this Transaction entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TransactionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,                
                TransactionDate = entity.TransactionDate,
                TransactionTypeInOut = entity.TransactionTypeInOut,                
                Detail = entity.Detail.Select(d => d.AdaptTransactionDetailToDtoComplete()).ToList()
            };
        }

        public static Transaction AdaptToTransaction(this TransactionDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Transaction
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,                
                BranchOfficeId = dto.BranchOfficeId,
                BranchOfficeStaffId = dto.BranchOfficeStaffId,
                BoxId = dto.BoxId,
                BuyId = dto.BuyId,
                SaleId = dto.SaleId,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivatedNote = dto.DeactivatedNote,
                TransactionDate = dto.TransactionDate,
                TransactionTypeInOut = dto.TransactionTypeInOut,                                                                 
                Detail = dto.Detail.Select(d => d.AdaptToTransactionDetail()).ToList()                
            };
        }

        public static TransactionDetail AdaptToTransactionDetail(this TransactionDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var detail = new TransactionDetail
            {
                Amount = dto.Amount,
                BillId = dto.Bill.Id,
                Id = dto.Id
            };

            detail.GenerateNewIdentity();

            return detail;
        }

        public static void AdaptToTransaction(this TransactionDto dto, Transaction to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.BoxId = dto.BoxId;
            to.SaleId = dto.SaleId;
            to.BuyId = dto.BuyId;
            to.BranchOfficeId = dto.BranchOffice.Id;
            to.BranchOfficeStaffId = dto.BranchOfficeStaff.Id;
            to.DeactivatedDate = dto.DeactivatedDate;
            to.DeactivatedNote = dto.DeactivatedNote;
            to.TransactionTypeInOut = dto.TransactionTypeInOut;
            to.TransactionDate = dto.TransactionDate;
            to.Description = dto.Description;
            to.Name = dto.Name;            
        }

        public static void AdaptToTransactionDetail(this TransactionDetailDto dto, TransactionDetail to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.Amount = dto.Amount;
            to.BillId = dto.Bill.Id;
            to.Bill = dto.Bill.AdaptToBill();
            to.TransactionId = dto.Transaction.Id;            
        }
    }
}
