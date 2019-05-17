namespace SAC.Stock.Domain.StockContext
{
    using SAC.Stock.Domain.BuyContext;
    using SAC.Stock.Domain.ProductContext;
    using SAC.Stock.Domain.SaleContext;
    using SAC.Stock.Service.StockContext;
    using System.Linq;

    internal static class StockAdapter
    {
        internal static StockDto AdpatToStockDto(this Stock entity)
        {
            if (entity == null)
            {               
                return null;                
            }

            return new StockDto
            {
                BranchOfficeId = entity.BranchOffice?.Id,
                BranchOfficeStaffId = entity.BranchOfficeStaff?.Id,
                DeactivatedDate = entity.DeactivatedDate,
                DeactivatedNote = entity.DeactivatedNote,
                UserId = entity.UserId,
                Id = entity.Id,
                StockDate = entity.StockDate,
                Detail = entity.Detail.Select(d => d.AdaptStockDetailToDto()).ToList(),
                Buys = entity.Buys?.Select(b => b.AdaptToBuyDtoIncomplete()).ToList(),
                Sales = entity.Sales?.Select(s => s.AdaptToSaleDtoIncomplete()).ToList()
            };
        }


        internal static StockDetailDto AdaptStockDetailToDto(this StockDetail entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new StockDetailDto
            {
                Amount = entity.Amount,
                ProductId = entity.ProductId,
                Product = entity.Product.AdaptToProductDto()
            };
        }
    }
}
