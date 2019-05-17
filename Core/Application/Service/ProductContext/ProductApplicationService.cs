namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.ProductContext;    
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;    
    internal class ProductApplicationService : IProductApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public ContainerDto AddContainer(ContainerDto containerInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.AddContainer(containerInfo).AdaptToContainerDto();
        }

        public ContainerDto ModifyContainer(ContainerDto containerInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.ModifyContainer(containerInfo).AdaptToContainerDto();
        }

        public ContainerDto GetContainer(int containerId)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetContainer(containerId).AdaptToContainerDto();
        }

        public ContainerDto GetContainerByValue(int value)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetContainerByValue(value).AdaptToContainerDto();
        }

        public IEnumerable<ContainerDto> QueryContainerByValue(int value)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryContainerByValue(value).Select(c => c.AdaptToContainerDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<ContainerDto> QueryContainer(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryContainer(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToContainerDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public AreaDto GetArea(int areaId)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetArea(areaId).AdaptToAreaDto();
        }
        public AreaDto AddArea(AreaDto areaInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.AddArea(areaInfo).AdaptToAreaDto();
        }
        public AreaDto ModifyArea(AreaDto AreaInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.ModifyArea(AreaInfo).AdaptToAreaDto();
        }

        public CategoryDto GetCategory(int categoryId)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetCategory(categoryId).AdaptToCategoryDto();
        }
        public CategoryDto AddCategory(CategoryDto categoryInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.AddCategory(categoryInfo).AdaptToCategoryDtoImcomplete();
        }
        public CategoryDto ModifyCategory(CategoryDto categoryInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.ModifyCategory(categoryInfo).AdaptToCategoryDtoImcomplete();
        }

        public SubCategoryDto GetSubCategory(int subCategoryId)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetSubCategory(subCategoryId).AdaptToSubCategoryDto();
        }
        public SubCategoryDto AddSubCategory(SubCategoryDto subCategoryInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.AddSubCategory(subCategoryInfo).AdaptToSubCategoryDtoIncomplete();
        }
        public SubCategoryDto ModifySubCategory(SubCategoryDto subCategoryInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.ModifySubCategory(subCategoryInfo).AdaptToSubCategoryDtoIncomplete();
        }

        public ProductPriceDto GetProductPrice(int productPriceId)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetProductPrice(productPriceId).AdaptToProductPriceDto();
        }
        public ProductPriceDto AddProductPrice(ProductPriceDto productPriceInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.AddProductPrice(productPriceInfo).AdaptToProductPriceDto();
        }
        public ProductPriceDto ModifyProductPrice(ProductPriceDto productPriceInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.ModifyProductPrice(productPriceInfo).AdaptToProductPriceDto();
        }

        public ProductDto GetProduct(int productId)
        {
            var svc = new ProductService(StockCtx);
            return svc.GetProduct(productId).AdaptToProductDto();
        }
        public ProductDto AddProduct(ProductDto productInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.AddProduct(productInfo).AdaptToProductDto();
        }
        public ProductDto ModifyProduct(ProductDto productInfo)
        {
            var svc = new ProductService(StockCtx);
            return svc.ModifyProduct(productInfo).AdaptToProductDto();
        }

        public ICollection<ProductDto> QueryProductByStock(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryProductByStock(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToProductDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }
        public ICollection<ProductDto> QueryProduct(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryProduct(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToProductDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<AreaDto> QueryArea(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryArea(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToAreaDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<CategoryDto> QueryCategory(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryCategory(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToCategoryDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<SubCategoryDto> QuerySubCategory(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QuerySubCategory(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToSubCategoryDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<ProductPriceDto> QueryProductPrice(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProductService(StockCtx);
                return svc.QueryProductPrice(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToProductPriceDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }   
    }
}
