using SAC.Stock.Service.ProductContext;
using System.Collections.Generic;
using System.Linq;

namespace SAC.Stock.Front.Models.Product
{
    internal static class ProductDtoAdapter
    {
        public static ProductDpo AdaptToProductDpo(this ProductDto product)
        {
            if (product == null)
            {
                return null;
            }

            return new ProductDpo
            {
                Code = product.Code,
                Description = product.Description,
                Id = product.Id,
                DisabledDate = product.DisabledDate,
                DisableNote = product.DisableNote,
                ForSale = product.ForSale,
                Name = product.Name,
                Container = new ContainerDpo
                {
                    Id = product.Id,
                    Amount = product.Container.Amount,
                    ParentContainer = new ContainerDpo
                    {
                        Id = product.Container.ParentContainer.Id,
                        Amount = product.Container.ParentContainer.Amount,
                        Name = product.Container.ParentContainer.Name
                    }
                },
                CurrentProductPrice = new ProductPriceDpo
                {
                    Id = product.ProductPrices.OrderByDescending(c => c.CreatedDate).FirstOrDefault().Id,
                    BuyMayorPrice = product.ProductPrices.OrderByDescending(c => c.CreatedDate).FirstOrDefault().BuyMayorPrice,
                    MayorGainPercent = product.ProductPrices.OrderByDescending(c => c.CreatedDate).FirstOrDefault().MayorGainPercent,
                    MinorGainPercent = product.ProductPrices.OrderByDescending(c => c.CreatedDate).FirstOrDefault().MinorGainPercent
                },
                SubCategory = new SubCategoryDpo
                {
                    Id = product.SubCategory.Id,
                    Name = product.SubCategory.Name,
                    Category = new CategoryDpo
                    {
                        Id = product.SubCategory.Category.Id,
                        Name = product.SubCategory.Category.Name
                    }
                }
            };
        }

        public static AreaDpo AdaptAreaToDpo(this AreaDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new AreaDpo
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static AreaDto AdaptAreaToDto(this AreaDpo dpo)
        {

            if (dpo == null)
            {
                return null;
            }

            return new AreaDto
            {
                Id = dpo.Id,
                Name = dpo.Name
            };
        }

        public static CategoryDto AdaptCategoryToDto(this CategoryDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = dpo.Id,
                Name = dpo.Name,
                Area = dpo.Area.AdaptAreaToDto(),
                SubCategories = dpo.SubCategories?.Select(d => d.AdaptToSubCategoryDto()).ToList()
            };
        }

        public static SubCategoryDto AdaptToSubCategoryDto(this SubCategoryDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new SubCategoryDto
            {
                Id = dpo.Id,
                Name = dpo.Name,
                Category = new CategoryDto
                {
                    Id = dpo.Category.Id,
                    Name = dpo.Category.Name
                }
            };
        }

        public static ContainerDto AdaptContainerToDto(this ContainerDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new ContainerDto
            {
                Id = dpo.Id,
                Amount = dpo.Amount,
                Description = dpo.Description,
                Name = dpo.Name,
                ParentContainer = dpo.ParentContainer?.AdaptContainerToDto()
            };
        }

        public static ProductDto AdaptToProductDto(this ProductDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            return new ProductDto
            {
                Code = dpo.Code,
                Container = dpo.Container.AdaptContainerToDto(),
                Description = dpo.Description,
                DisabledDate = dpo.DisabledDate,
                DisableNote = dpo.DisableNote,
                ForSale = dpo.ForSale,
                Id = dpo.Id,
                Name = dpo.Name,
                SubCategory = dpo.SubCategory.AdaptToSubCategoryDto(),
                ProductPrices = new List<ProductPriceDto>
                {
                    new ProductPriceDto
                    {
                        BuyMayorPrice = dpo.CurrentProductPrice.BuyMayorPrice,
                        CreatedDate = dpo.CurrentProductPrice.CreatedDate,
                        DeactivatorUserId = dpo.CurrentProductPrice.DeactivatorUserId,
                        DisabledDate = dpo.CurrentProductPrice.DisabledDate,
                        DisableNote = dpo.CurrentProductPrice.DisableNote,
                        //Id = dpo.CurrentProductPrice.Id,
                        MayorGainPercent = dpo.CurrentProductPrice.MayorGainPercent,
                        MinorGainPercent = dpo.CurrentProductPrice.MinorGainPercent,
                        UserId = dpo.CurrentProductPrice.UserId
                    }
                }
            };
        }
    }
}