namespace SAC.Stock.Domain.SaleContext
{
    using BranchOfficeContext;
    using CustomerContext;
    using ProductContext;
    using TransactionContext;
    using Service.SaleContext;
    using System.Linq;
    using SAC.Stock.Domain.BoxContext;
    using SAC.Stock.Domain.StockContext;

    internal static class SaleAdapter
    {
        public static SaleDto AdaptToSaleDto(this Sale entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SaleDto
            {
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = entity.BranchOfficeStaff.AdaptToFranchiseStaffDto(),
                Customer = entity.Customer.AdaptToCustomerDto(),
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,
                Detail = entity.Detail.Select(d => d.AdaptToSaleDetailDto()).ToList(),
                Id = entity.Id,
                PaymentTypeCode = entity.PaymentTypeCode,                
                PreSale = entity.PreSale,
                MayorMinorSale = entity.MayorMinorSale,
                Box = entity.Box?.AdaptBoxToDto(),
                Stock = entity.Stock?.AdpatToStockDto(),
                SaleDate = entity.SaleDate,
                Transactions = entity.Transactions?.Select(t => t.AdaptTransactionToDto()).ToList()
            };
        }

        public static SaleDto AdaptToSaleDtoIncomplete(this Sale entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SaleDto
            {
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto(),
                BranchOfficeStaff = entity.BranchOfficeStaff.AdaptToFranchiseStaffDto(),
                Customer = entity.Customer.AdaptToCustomerDto(),
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,
                Detail = entity.Detail.Select(d => d.AdaptToSaleDetailDto()).ToList(),
                Id = entity.Id,
                PreSale = entity.PreSale,
                MayorMinorSale = entity.MayorMinorSale,
                PaymentTypeCode = entity.PaymentTypeCode,                
                SaleDate = entity.SaleDate,
                Transactions = entity.Transactions?.Select(t => t.AdaptTransactionToDto()).ToList()
            };
        }

        public static SaleDetailDto AdaptToSaleDetailDto(this SaleDetail entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SaleDetailDto
            {
                Amount = entity.Amount,
                Id = entity.Id,
                Price = entity.Price,
                Product = entity.Product.AdaptToProductDto(),
                ProductId = entity.ProductId
            };
        }

        public static Sale AdaptToSale(this SaleDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Sale
            {
                BranchOffice = dto.BranchOffice.AdapterToBranchOffice(),
                BranchOfficeId = dto.BranchOffice.Id,
                Id = dto.Id,
                BranchOfficeStaff = dto.BranchOfficeStaff.AdaptToBranchOfficeStaff(),
                BranchOfficeStaffId = dto.BranchOfficeStaff.Id,
                Customer = dto.Customer.AdaptToCustomer(),
                CustomerId = dto.Customer.Id,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivatedNote = dto.DeactivatedNote,
                PaymentTypeCode = dto.PaymentTypeCode,
                PreSale = dto.PreSale,
                MayorMinorSale = dto.MayorMinorSale,
                SaleDate = dto.SaleDate,
                BoxId = dto.Box?.Id,
                StockId = dto.Stock?.Id,
                Detail = dto.Detail.Select(d => d.AdaptToSaleDetail()).ToList(),
                Transactions = dto.Transactions?.Select(t => t.AdaptToTransaction()).ToList()
            };
        }

        public static SaleDetail AdaptToSaleDetail(this SaleDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new SaleDetail
            {
                Amount = dto.Amount,
                Id = dto.Id,
                Price = dto.Price,
                ProductId = dto.Product.Id,
                SaleId = dto.Sale.Id
            };
        }
    }
}