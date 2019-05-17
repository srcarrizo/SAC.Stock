namespace SAC.Stock.Front.Models.Buy
{
    using BranchOffice;
    using Product;
    using Provinder;
    using Transaction;
    using Service.BuyContext;    
    using System.Linq;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.ProviderContext;
    using SAC.Stock.Service.TransactionContext;
    using SAC.Stock.Service.BillContext;
    using SAC.Stock.Service.ProductContext;
    using System.Collections.Generic;

    internal static class BuyDtoAdapter
    {
        public static BuyDpo AdaptToBuyDpo(this BuyDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BuyDpo
            {                
                Id = dto.Id,
                BranchOffice = dto.BranchOffice.AdaptToBranchOfficeDpo(),
                BranchOfficeStaff = dto.BranchOfficeStaff.AdaptToBranchOfficeStaffDpo(),
                BuyDate = dto.BuyDate,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivatedNote = dto.DeactivatedNote,
                PaymentTypeCode = dto.PaymentTypeCode,
                Provider = dto.Provider.AdaptToProviderDpo(),
                BoxId = dto.Box?.Id,
                StockId = dto.Stock?.Id,
                Transactions = dto.Transactions?.Select(t => t.AdaptToTransactionDpo()).ToList(),
                Detail = dto.Detail.Select(d => d.AdaptToBuyDetailDpo()).ToList()                
            };
        }

        public static BuyDetailDpo AdaptToBuyDetailDpo(this BuyDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BuyDetailDpo
            {
                Id = dto.Id,
                Amount = dto.Amount,
                Price = dto.Price,
                Product = dto.Product.AdaptToProductDpo()
            };
        }

        public static BuyDto AdaptToBuyDto(this BuyDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new BuyDto
            {
                Id = dpo.Id,
                DeactivatedDate = dpo.DeactivatedDate,
                DeactivatedNote = dpo.DeactivatedNote,
                BranchOffice = new BranchOfficeDto
                {
                    Id = dpo.BranchOffice.Id
                },
                BranchOfficeStaff = new BranchOfficeStaffDto
                {
                    Id = dpo.BranchOfficeStaff.Id
                },
                BuyDate = dpo.BuyDate,
                PaymentTypeCode = dpo.PaymentTypeCode,
                Provider = new ProviderDto
                {
                    Id = dpo.Provider.Id,
                    Name = dpo.Provider.Name
                },
                Transactions = dpo.Transactions?.Select(t => new TransactionDto
                {
                    TransactionDate = t.TransactionDate,
                    TransactionTypeInOut = t.TransactionTypeInOut,
                    BranchOffice = new BranchOfficeDto { Id = dpo.BranchOffice.Id },
                    BranchOfficeStaff = new BranchOfficeStaffDto { Id = dpo.BranchOfficeStaff.Id },
                    Detail = t.Detail.Select(d => new TransactionDetailDto { Amount = d.Amount, Bill = new BillDto { Id = d.Bill.Id } }).ToList()
                }).ToList(),
                Detail = dpo.Detail.Select(r => new BuyDetailDto
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Price = r.Price,
                    Product = new ProductDto
                    {
                        Id = r.Product.Id,
                        Code = r.Product.Code,
                        Name = r.Product.Name,
                        Container = new ContainerDto
                        {
                            Amount = r.Product.Container.Amount,
                            ParentContainer = new ContainerDto
                            {
                                Amount = r.Product.Container.ParentContainer.Amount
                            }
                        },
                        ProductPrices = new List<ProductPriceDto>
                        {
                            new ProductPriceDto
                            {
                                BuyMayorPrice = r.Product.CurrentProductPrice.BuyMayorPrice
                            }                            
                        },
                        SubCategory = new SubCategoryDto { Name = r.Product.SubCategory.Name } }
                }).ToList()
            };
        }
    }
}