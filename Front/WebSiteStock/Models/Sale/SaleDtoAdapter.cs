namespace SAC.Stock.Front.Models.Sale
{
    using BranchOffice;
    using Product;    
    using Transaction;
    using Service.SaleContext;    
    using System.Linq;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.CustomerContext;
    using SAC.Stock.Service.TransactionContext;
    using SAC.Stock.Service.BillContext;
    using SAC.Stock.Service.ProductContext;
    using System.Collections.Generic;
    using SAC.Stock.Front.Models.Customer;

    internal static class SaleDtoAdapter
    {
        public static SaleDpo AdaptToSaleDpo(this SaleDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new SaleDpo
            {                
                Id = dto.Id,
                BranchOffice = dto.BranchOffice.AdaptToBranchOfficeDpo(),
                BranchOfficeStaff = dto.BranchOfficeStaff.AdaptToBranchOfficeStaffDpo(),                
                SaleDate = dto.SaleDate,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivatedNote = dto.DeactivatedNote,
                PaymentTypeCode = dto.PaymentTypeCode,
                PreSale = dto.PreSale,
                MayorMinorSale = dto.MayorMinorSale,
                BoxId = dto.Box?.Id,
                StockId = dto.Stock?.Id,
                Customer = dto.Customer.AdaptToCustomerDpo(),
                Transactions = dto.Transactions?.Select(t => t.AdaptToTransactionDpo()).ToList(),                
                Detail = dto.Detail.Select(d => d.AdaptToSaleDetailDpo()).ToList()                
            };
        }

        public static SaleDetailDpo AdaptToSaleDetailDpo(this SaleDetailDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new SaleDetailDpo
            {
                Id = dto.Id,
                Amount = dto.Amount,
                Price = dto.Price,
                Product = dto.Product.AdaptToProductDpo()
            };
        }

        public static SaleDto AdaptToSaleDto(this SaleDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new SaleDto
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
                SaleDate = dpo.SaleDate,
                PaymentTypeCode = dpo.PaymentTypeCode,
                PreSale = dpo.PreSale,
                MayorMinorSale = dpo.MayorMinorSale,
                Customer = new CustomerDto
                {
                    Id = dpo.Customer.Id,
                    Name = dpo.Customer.Name
                },
                Transactions = dpo.Transactions?.Select(t => new TransactionDto
                {
                    TransactionDate = t.TransactionDate,
                    TransactionTypeInOut = t.TransactionTypeInOut,
                    BranchOffice = new BranchOfficeDto { Id = dpo.BranchOffice.Id },
                    BranchOfficeStaff = new BranchOfficeStaffDto { Id = dpo.BranchOfficeStaff.Id },
                    Detail = t.Detail.Select(d => new TransactionDetailDto { Amount = d.Amount, Bill = new BillDto { Id = d.Bill.Id } }).ToList()
                }).ToList(),
                Detail = dpo.Detail.Select(r => new SaleDetailDto
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
                                Id = r.Product.CurrentProductPrice.Id,
                                BuyMayorPrice = r.Product.CurrentProductPrice.BuyMayorPrice,
                                MayorGainPercent = r.Product.CurrentProductPrice.MayorGainPercent,
                                MinorGainPercent = r.Product.CurrentProductPrice.MinorGainPercent
                            }                            
                        },
                        SubCategory = new SubCategoryDto { Name = r.Product.SubCategory.Name } }
                }).ToList()
            };
        }
    }
}