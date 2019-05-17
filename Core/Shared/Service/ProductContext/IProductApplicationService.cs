namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Data;    
    using System.Collections.Generic;    
    using System.ServiceModel;
    
    [ServiceContract]
    internal interface IProductApplicationService
    {
        [OperationContract]
        ContainerDto AddContainer(ContainerDto containerInfo);
        [OperationContract]
        ContainerDto ModifyContainer(ContainerDto containerInfo);
        [OperationContract]
        ContainerDto GetContainer(int containerId);
        [OperationContract]
        ContainerDto GetContainerByValue(int value);
        [OperationContract]
        ICollection<ContainerDto> QueryContainer(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);
        [OperationContract]
        IEnumerable<ContainerDto> QueryContainerByValue(int value);

        [OperationContract]
        ProductDto AddProduct(ProductDto productInfo);
        [OperationContract]
        ProductDto ModifyProduct(ProductDto productInfo);
        [OperationContract]
        ProductDto GetProduct(int productId);
        [OperationContract]
        ICollection<ProductDto> QueryProduct(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);
        [OperationContract]
        ICollection<ProductDto> QueryProductByStock(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);        

        [OperationContract]
        AreaDto AddArea(AreaDto areaInfo);
        [OperationContract]
        AreaDto ModifyArea(AreaDto areaInfo);
        [OperationContract]
        AreaDto GetArea(int areaId);
        [OperationContract]
        ICollection<AreaDto> QueryArea(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);

        [OperationContract]
        CategoryDto AddCategory(CategoryDto categoryInfo);
        [OperationContract]
        CategoryDto ModifyCategory(CategoryDto categoryInfo);
        [OperationContract]
        CategoryDto GetCategory(int categoryId);
        [OperationContract]
        ICollection<CategoryDto> QueryCategory(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);

        [OperationContract]
        SubCategoryDto AddSubCategory(SubCategoryDto subCategoryInfo);
        [OperationContract]
        SubCategoryDto ModifySubCategory(SubCategoryDto subCategoryInfo);
        [OperationContract]
        SubCategoryDto GetSubCategory(int subCategoryId);
        [OperationContract]
        ICollection<SubCategoryDto> QuerySubCategory(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);

        [OperationContract]
        ProductPriceDto AddProductPrice(ProductPriceDto productPriceInfo);
        [OperationContract]
        ProductPriceDto ModifyProductPrice(ProductPriceDto productPriceInfo);
        [OperationContract]
        ProductPriceDto GetProductPrice(int productPriceId);
        [OperationContract]
        ICollection<ProductPriceDto> QueryProductPrice(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);
    }
}
