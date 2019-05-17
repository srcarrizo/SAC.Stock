namespace SAC.Stock.Domain.BuyContext
{
    using BranchOfficeContext;
    using ProviderContext;
    using Service.BuyContext;    
    using System.Linq;
    using ProductContext;
    using TransactionContext;
    using StockContext;
    using BoxContext;

    internal static class BuyAdapter
    {
        public static BuyDto AdaptToBuyDto(this Buy entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BuyDto
            {
                Id = entity.Id,
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = entity.BranchOfficeStaff.AdaptToFranchiseStaffDto(),
                BuyDate = entity.BuyDate,
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,
                Detail = entity.Detail.Select(d => d.AdaptBuyDetailToDto()).ToList(),
                PaymentTypeCode = entity.PaymentTypeCode,
                Box = entity.Box?.AdaptBoxToDto(),
                Stock = entity.Stock?.AdpatToStockDto(),
                Provider = entity.Provider.AdaptToProviderDto(),
                Transactions = entity.Transactions?.Select(t => t.AdaptTransactionToDto()).ToList()                
            };
        }

        public static BuyDto AdaptToBuyDtoIncomplete(this Buy entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BuyDto
            {
                Id = entity.Id,
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = entity.BranchOfficeStaff.AdaptToFranchiseStaffDto(),
                BuyDate = entity.BuyDate,
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,
                Detail = entity.Detail.Select(d => d.AdaptBuyDetailToDto()).ToList(),
                PaymentTypeCode = entity.PaymentTypeCode,                
                Provider = entity.Provider.AdaptToProviderDto(),
                Transactions = entity.Transactions?.Select(t => t.AdaptTransactionToDto()).ToList()
            };
        }

        public static BuyDetailDto AdaptBuyDetailToDto(this BuyDetail entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BuyDetailDto
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Price = entity.Price,
                Product = entity.Product.AdaptToProductDto(),
                BuyId = entity.BuyId
            };
        }


        public static Buy AdaptToBuy(this BuyDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Buy
            {
                Id = dto.Id,
                BranchOffice = dto.BranchOffice.AdapterToBranchOffice(),
                BranchOfficeId = dto.BranchOffice.Id,
                BranchOfficeStaff = dto.BranchOfficeStaff.AdaptToBranchOfficeStaff(),
                BranchOfficeStaffId = dto.BranchOfficeStaff.Id,
                BuyDate = dto.BuyDate,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivatedNote = dto.DeactivatedNote,
                PaymentTypeCode = dto.PaymentTypeCode,
                Provider = dto.Provider.AdaptToProvider(),
                ProviderId = dto.Provider.Id,
                BoxId = dto.Box?.Id,
                StockId = dto.Stock?.Id,
                Transactions = dto.Transactions?.Select(d => d.AdaptToTransaction()).ToList(),
                Detail = dto.Detail.Select(d => d.AdaptToBuyDetail()).ToList()                
            };
        }

        public static BuyDetail AdaptToBuyDetail(this BuyDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BuyDetail
            {
                Id = dto.Id,
                Amount = dto.Amount,
                BuyId = dto.BuyId,
                Price = dto.Price,
                ProductId = dto.Product.Id
            };
        }
    }
}
