namespace SAC.Stock.Domain.ProductContext
{
    using Service.ProductContext;    
    using System.Linq;
    internal static class ProductAdapter
    {
        public static Product AdaptToProduct(this ProductDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Product
            {
                Description = dto.Description,
                DisableNote = dto.DisableNote,
                DisabledDate = dto.DisabledDate,
                ForSale = dto.ForSale,
                Id = dto.Id,
                Name = dto.Name,
                ContainerId = dto.Container.Id,
                SubCategoryId = dto.SubCategory.Id,
                Code = dto.Code,
                ProductPrices = dto.ProductPrices?.Select(p => p.AdaptToProductPrice()).ToList()
            };            
        }

        public static ProductDto AdaptToProductDto(this Product entity)
        {
            if (entity == null)
            {
                return null;
            }

            var product = new ProductDto
            {
                Description = entity.Description,
                DisableNote = entity.DisableNote,
                DisabledDate = entity.DisabledDate,
                ForSale = entity.ForSale,
                Id = entity.Id,
                Name = entity.Name,
                Container = entity.Container != null ? new ContainerDto
                {
                    Id = entity.Container.Id,
                    Amount = entity.Container.Amount,
                    Description = entity.Container.Description,
                    Name = entity.Container.Name,
                    ParentContainer = entity.Container.ParentContainer != null ? new ContainerDto
                    {
                        Id = entity.Container.ParentContainer.Id,
                        Amount = entity.Container.ParentContainer.Amount,
                        Description = entity.Container.ParentContainer.Description,
                        Name = entity.Container.ParentContainer.Name
                    } : null
                } : null,
                SubCategory = entity.SubCategory != null ? new SubCategoryDto
                {
                    Id = entity.SubCategory.Id,
                    Name = entity.SubCategory.Name,
                    Category = entity.SubCategory.Category != null ? new CategoryDto
                    {
                        Id = entity.SubCategory.Category.Id,
                        Name = entity.SubCategory.Category.Name
                    } : null
                } : null,
                ProductPrices = entity.ProductPrices?.Select(c => c.AdaptToProductPriceDto()).ToList(),
                Code = entity.Code
            };           

            return product;
        }

        public static SubCategory AdaptToSubCategory(this SubCategoryDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new SubCategory
            {
                Id = dto.Id,                
                CategoryId = dto.Category.Id,
                Name = dto.Name                
            };
        }

        public static SubCategoryDto AdaptToSubCategoryDto(this SubCategory entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SubCategoryDto
            {
                Id = entity.Id,
                Category = new CategoryDto
                {
                    Id = entity.Category.Id,
                    Name = entity.Category.Name                    
                }, 
                Name = entity.Name,
                Products = entity.Products?.Select(p => p.AdaptToProductDto()).ToList()                
            };
        }

        public static SubCategoryDto AdaptToSubCategoryDtoIncomplete(this SubCategory entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SubCategoryDto
            {
                Id = entity.Id,                
                Name = entity.Name,
                Products = entity.Products?.Select(p => p.AdaptToProductDto()).ToList()
            };
        }

        public static Category AdaptToCategory(this CategoryDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Category
            {
                Id = dto.Id,
                AreaId = dto.Area.Id,                
                Name = dto.Name                
            };
        }

        public static CategoryDto AdaptToCategoryDto(this Category entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = entity.Id,                
                Area = new AreaDto
                {
                    Id = entity.Area.Id,
                    Name = entity.Area.Name                    
                },
                Name = entity.Name,
                SubCategories = entity.SubCategories?.Select(s => s.AdaptToSubCategoryDto()).ToList()                
            };
        }

        public static CategoryDto AdaptToCategoryDtoImcomplete(this Category entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = entity.Id,                
                Name = entity.Name,
                SubCategories = entity.SubCategories?.Select(s => s.AdaptToSubCategoryDto()).ToList()
            };
        }

        public static Area AdaptToArea(this AreaDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Area
            {
                Id = dto.Id,                
                Name = dto.Name                          
            };
        }        

        public static AreaDto AdaptToAreaDto(this Area entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AreaDto
            {
                Id = entity.Id,               
                Name = entity.Name,
                Categories = entity.Categories?.Select(s => s.AdaptToCategoryDto()).ToList()
            };
        }

        public static ProductPrice AdaptToProductPrice(this ProductPriceDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var productPrice = new ProductPrice
            {
                Id = dto.Id,
                BuyMayorPrice = dto.BuyMayorPrice,
                CreatedDate = dto.CreatedDate,
                DeactivatorUserId = dto.DeactivatorUserId,
                DisabledDate = dto.DisabledDate,
                DisableNote = dto.DisableNote,
                MayorGainPercent = dto.MayorGainPercent,
                MinorGainPercent = dto.MinorGainPercent,                                
                UserId = dto.UserId
            };

            if (dto.Product != null)
            {
                productPrice.ProductId = dto.Product.Id;
            }

            return productPrice;
        }

        public static ProductPriceDto AdaptToProductPriceDto(this ProductPrice entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ProductPriceDto
            {
                Id = entity.Id,
                BuyMayorPrice = entity.BuyMayorPrice,
                CreatedDate = entity.CreatedDate,
                DeactivatorUserId = entity.DeactivatorUserId,
                DisabledDate = entity.DisabledDate,
                DisableNote = entity.DisableNote,
                MayorGainPercent = entity.MayorGainPercent,
                MinorGainPercent = entity.MinorGainPercent,
                Product =  new ProductDto
                {
                    Id = entity.Product.Id,
                    Description = entity.Product.Description,
                    DisabledDate = entity.Product.DisabledDate,
                    DisableNote = entity.Product.DisableNote,
                    Name = entity.Product.Name,
                    ForSale = entity.Product.ForSale,
                    Code = entity.Product.Code
                },                               
                UserId = entity.UserId                
            };
        }
        
        public static void AdaptToProduct(this ProductDto dto, Product to)
        {
            if (dto == null || to == null)
            {
                return;
            }

            to.Description = dto.Description;
            to.DisableNote = dto.DisableNote;
            to.DisabledDate = dto.DisabledDate;
            to.ForSale = dto.ForSale;
            to.Id = dto.Id;
            to.Name = dto.Name;            
            to.ContainerId = dto.Container.Id;            
            to.SubCategoryId = dto.SubCategory.Id;
            to.Code = dto.Code;           
            to.ProductPrices.Add(dto.ProductPrices.FirstOrDefault().AdaptToProductPrice());
        }

        public static void AdaptToSubCategory(this SubCategoryDto dto, SubCategory to)
        {
            if (dto == null || to == null)
            {
                return;
            }

            to.Id = dto.Id;            
            to.CategoryId = dto.Category.Id;
            to.Name = dto.Name;            
        }

        public static void AdaptToCategory(this CategoryDto dto, Category to)
        {
            if (dto == null || to == null)
            {
                return;
            }

            to.Id = dto.Id;
            to.AreaId = dto.Area.Id;            
            to.Name = dto.Name;            
        }

        public static void AdaptToArea(this AreaDto dto, Area to)
        {
            if (dto == null || to == null)
            {
                return;
            }

            to.Id = dto.Id;
            to.Name = dto.Name;            
        }

        public static void AdaptToProductPrice(this ProductPriceDto dto, ProductPrice to)
        {
            if (dto == null || to == null)
            {
                return;
            }

            to.Id = dto.Id;
            to.BuyMayorPrice = dto.BuyMayorPrice;
            to.CreatedDate = dto.CreatedDate;
            to.DeactivatorUserId = dto.DeactivatorUserId;
            to.DisabledDate = dto.DisabledDate;
            to.DisableNote = dto.DisableNote;
            to.MayorGainPercent = dto.MayorGainPercent;
            to.MinorGainPercent = dto.MinorGainPercent;
            to.ProductId = dto.Product.Id;
            to.UserId = dto.UserId;
        }
    }
}
