namespace SAC.Stock.Domain.BoxContext
{
    using BillContext;
    using TransactionContext;
    using Service.BoxContext;
    using System.Linq;
    using SAC.Stock.Domain.BuyContext;
    using SAC.Stock.Domain.SaleContext;

    internal static class BoxAdapter
    {
        public static BoxDto AdaptBoxToDto(this Box entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BoxDto
            {
                CloseDate = entity.CloseDate,
                CloseNote = entity.CloseNote,
                DeactivateDate = entity.DeactivateDate,
                DeactivateNote = entity.DeactivateNote,
                Detail = entity.Detail.Select(d => d.AdaptBoxDetailToDto()).ToList(),
                Id = entity.Id,
                Withdrawal = entity.Withdrawal,
                OpenDate = entity.OpenDate,
                OpenNote = entity.OpenNote,                  
                Transactions = entity.Transactions?.Select(t => t.AdaptTransactionToDto()).ToList(),
                Buys = entity.Buys?.Select(b => b.AdaptToBuyDtoIncomplete()).ToList(),
                Sales = entity.Sales?.Select(s => s.AdaptToSaleDtoIncomplete()).ToList()
            };
        }

        public static BoxDetailDto AdaptBoxDetailToDto(this BoxDetail entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BoxDetailDto
            {
                Amount = entity.Amount,
                Bill = entity.Bill.AdaptToBillDto(),
                Id = entity.Id,
                Box = new BoxDto
                {
                    Id = entity.BoxId
                }
            };
        }

        public static Box AdaptToBox(this BoxDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Box
            {
                Id = dto.Id,
                CloseDate = dto.CloseDate,
                CloseNote = dto.CloseNote,
                DeactivateDate = dto.DeactivateDate,
                DeactivateNote = dto.DeactivateNote,
                Withdrawal = dto.Withdrawal,
                OpenDate = dto.OpenDate,
                OpenNote = dto.OpenNote,                
                Detail = dto.Detail.Select(d => d.AdaptToBoxDetail()).ToList()            
            };
        }

        public static BoxDetail AdaptToBoxDetail(this BoxDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BoxDetail
            {
                Id = dto.Id,
                Amount = dto.Amount,
                BillId = dto.Bill.Id                
            };
        }
    }
}