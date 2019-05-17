namespace SAC.Stock.Front.Models.Box
{       
    using Transaction;    
    using System.Linq;    
    using SAC.Stock.Service.TransactionContext;
    using SAC.Stock.Service.BillContext;    
    using SAC.Stock.Service.BoxContext;
    using SAC.Stock.Front.Models.Bill;

    internal static class BoxDtoAdapter
    {
        public static BoxDpo AdaptToBoxDpo(this BoxDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var dpo = new BoxDpo
            {                
                Id = dto.Id,
                CloseDate = dto.CloseDate,
                CloseNote = dto.CloseNote,
                DeactivateDate = dto.DeactivateDate,
                DeactivateNote = dto.DeactivateNote,
                Withdrawal = dto.Withdrawal,
                OpenDate = dto.OpenDate,
                OpenNote = dto.OpenNote,                
                Detail = dto.Detail.Select(d => d.AdaptBoxDetailToDpo()).ToList(),
                Transactions = dto.Transactions?.Select(t => t.AdaptToTransactionDpo()).ToList()
            };

            if (dto.OpenDate.HasValue && !dto.CloseDate.HasValue)
            {
                dpo.OpeningClosing = true;
            }
            else if (!dto.OpenDate.HasValue && dto.CloseDate.HasValue)
            {
                dpo.OpeningClosing = false;
            }

            return dpo;
        }

        public static BoxDetailDpo AdaptBoxDetailToDpo(this BoxDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BoxDetailDpo
            {
                Id = dto.Id,
                Amount = dto.Amount,
                Bill = dto.Bill.AdaptToBillDpo()
            };
        }

        public static BoxDto AdaptToBoxDto(this BoxDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new BoxDto
            {
                Id = dpo.Id,
                DeactivateDate = dpo.DeactivateDate,
                DeactivateNote = dpo.DeactivateNote,              
                Withdrawal = dpo.Withdrawal,
                OpenDate = dpo.OpenDate,
                OpenNote = dpo.OpenNote,
                CloseDate = dpo.CloseDate,
                CloseNote = dpo.CloseNote,                                                               
                Detail = dpo.Detail.Select(d => new BoxDetailDto
                {
                    Id = d.Id,
                    Amount = d.Amount,
                    Bill = new BillDto
                    {
                        Id = d.Bill.Id,
                        Code = d.Bill.Code,
                        Name = d.Bill.Name,
                        Value = d.Bill.Value,
                        BillUnitType = new BillUnitTypeDto
                        {
                            Id = d.Bill.BillUnitType.Id,
                            Code = d.Bill.BillUnitType.Code,
                            Name = d.Bill.BillUnitType.Name,
                            IsDecimal = d.Bill.BillUnitType.IsDecimal
                        }
                    }
                }).ToList()                
            };
        }
    }
}