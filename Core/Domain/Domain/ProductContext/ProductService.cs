namespace SAC.Stock.Domain.ProductContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Data.Ordering;
    using SAC.Seed.NLayer.Domain;
    using SAC.Seed.NLayer.Domain.Specification;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.StockContext;
    using SAC.Stock.Service.ProductContext;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class ProductService
    {
        private readonly IDataContext context;

        public ProductService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Product, int> ViewProduct
        {
            get
            {
                return context.GetView<Product, int>();
            }
        }

        private IDataView<Area, int> ViewArea
        {
            get
            {
                return context.GetView<Area, int>();
            }
        }

        private IDataView<Category, int> ViewCategory
        {
            get
            {
                return context.GetView<Category, int>();
            }
        }

        private IDataView<SubCategory, int> ViewSubCategory
        {
            get
            {
                return context.GetView<SubCategory, int>();
            }
        }

        private IDataView<ProductPrice, int> ViewProductPrice
        {
            get
            {
                return context.GetView<ProductPrice, int>();
            }
        }

        private IDataView<Container, int> ViewContainer
        {
            get
            {
                return context.GetView<Container, int>();
            }
        }

        public Area AddArea(AreaDto areaInfo)
        {
            var area = NewArea(areaInfo);
            ViewArea.Add(area);
            context.ApplyChanges();

            return area;
        }

        private Area NewArea(AreaDto areaInfo)
        {
            TrimArea(areaInfo);
            var area = areaInfo.AdaptToArea();
            area.GenerateNewIdentity();

            return area;
        }

        public Category AddCategory(CategoryDto categoryInfo)
        {
            var category = NewCateogry(categoryInfo);            
            context.ApplyChanges();

            return category;
        }

        private Category NewCateogry(CategoryDto categoryInfo)
        {
            TrimCategory(categoryInfo);
            var category = categoryInfo.AdaptToCategory();            
            ViewCategory.Add(category);

            return category;
        }

        public SubCategory AddSubCategory(SubCategoryDto subCategoryInfo)
        {
            var subCategory = NewSubCateogry(subCategoryInfo);
            ViewSubCategory.Add(subCategory);
            context.ApplyChanges();

            return subCategory;
        }

        private SubCategory NewSubCateogry(SubCategoryDto subCategoryInfo)
        {
            TrimSubCategory(subCategoryInfo);
            var subCategory = subCategoryInfo.AdaptToSubCategory();
            subCategory.GenerateNewIdentity();

            return subCategory;
        }

        public ProductPrice AddProductPrice(ProductPriceDto productPriceInfo)
        {
            var productPrice = NewProductPrice(productPriceInfo);
            ViewProductPrice.Add(productPrice);
            context.ApplyChanges();

            return productPrice;
        }

        private ProductPrice NewProductPrice(ProductPriceDto productPriceInfo)
        {
            TrimProductPrice(productPriceInfo);
            var productPrice = productPriceInfo.AdaptToProductPrice();
            productPrice.GenerateNewIdentity();

            return productPrice;
        }

        public Product AddProduct(ProductDto productInfo)
        {
            var product = NewProduct(productInfo);
            ViewProduct.Add(product);
            context.ApplyChanges();

            return product;
        }

        private Product NewProduct(ProductDto productInfo)
        {
            TrimProduct(productInfo);
            var product = productInfo.AdaptToProduct();
            product.GenerateNewIdentity();

            return product;
        }

        public Product ModifyProduct(ProductDto productInfo)
        {
            var product = UpdateProduct(productInfo);
            ViewProduct.Modify(product);
            context.ApplyChanges();

            return product;
        }

        private Product UpdateProduct(ProductDto productInfo)
        {
            var product = GetProduct(productInfo.Id);
            if (product == null)
            {
                throw BusinessRulesCode.ProductNotExists.NewBusinessException();
            }

            TrimProduct(productInfo);
            productInfo.AdaptToProduct(product);

            if ((productInfo.DisabledDate != null) && (productInfo.DisableNote == null))
            {
                throw BusinessRulesCode.ProductDeactivateNote.NewBusinessException();
            }

            if (productInfo.DisableNote != null)
            {
                product.DisabledDate = new DateTimeOffset(DateTime.UtcNow);
            }

            return product;
        }

        public Area GetArea(int areaId)
        {
            return ViewArea.Get(areaId);
        }

        public Category GetCategory(int categoryId)
        {
            return ViewCategory.Get(categoryId);
        }

        public SubCategory GetSubCategory(int subCategoryId)
        {
            return ViewSubCategory.Get(subCategoryId);
        }

        public ProductPrice GetProductPrice(int productPriceId)
        {
            return ViewProductPrice.Get(productPriceId);
        }

        public Product GetProduct(int productId)
        {
            return ViewProduct.Get(productId);
        }

        public Container AddContainer(ContainerDto containerInfo)
        {
            var container = NewContainer(containerInfo);
            ViewContainer.Add(container);
            context.ApplyChanges();

            return container;
        }

        private Container NewContainer(ContainerDto containerInfo)
        {
            TrimContainer(containerInfo);
            var Container = containerInfo.AdaptToContainer();
            Container.GenerateNewIdentity();

            return Container;
        }

        public Container ModifyContainer(ContainerDto containerInfo)
        {
            var container = UpdateContainer(containerInfo);
            ViewContainer.Modify(container);
            context.ApplyChanges();
            return container;
        }

        private Container UpdateContainer(ContainerDto containerInfo)
        {
            var container = GetContainer(containerInfo.Id);
            if (container == null)
            {
                throw BusinessRulesCode.ContainerNotExists.NewBusinessException();
            }

            TrimContainer(containerInfo);
            containerInfo.AdaptToContainer(container);

            return container;
        }

        public Container GetContainer(int ContainerId)
        {
            return ViewContainer.Get(ContainerId);
        }

        public Container GetContainerByValue(int value)
        {
            return ViewContainer.GetFirst(c => c.Amount.Equals(value));
        }

        public IEnumerable<Container> QueryContainerByValue(int value)
        {
            return ViewContainer.Query(c => c.Amount.Equals(value));
        }
        
        public Area ModifyArea(AreaDto areaInfo)
        {
            var area = UpdateArea(areaInfo);
            ViewArea.Modify(area);
            context.ApplyChanges();

            return area;
        }

        private Area UpdateArea(AreaDto areaInfo)
        {
            var area = GetArea(areaInfo.Id);
            if (area == null)
            {
                throw BusinessRulesCode.AreaNotExists.NewBusinessException();
            }

            TrimArea(areaInfo);
            areaInfo.AdaptToArea(area);
            
            return area;
        }

        public Category ModifyCategory(CategoryDto categoryInfo)
        {
            var category = UpdateCategory(categoryInfo);
            ViewCategory.Modify(category);
            context.ApplyChanges();

            return category;
        }

        private Category UpdateCategory(CategoryDto categoryInfo)
        {
            var category = GetCategory(categoryInfo.Id);
            if (category == null)
            {
                throw BusinessRulesCode.CategoryNotExists.NewBusinessException();
            }

            TrimCategory(categoryInfo);
            categoryInfo.AdaptToCategory(category);
                    
            return category;
        }

        public SubCategory ModifySubCategory(SubCategoryDto subCategoryInfo)
        {
            var subCategory = UpdateSubCategory(subCategoryInfo);
            ViewSubCategory.Modify(subCategory);
            context.ApplyChanges();

            return subCategory;
        }

        private SubCategory UpdateSubCategory(SubCategoryDto subCategoryInfo)
        {
            var subCategory = GetSubCategory(subCategoryInfo.Id);
            if (subCategory == null)
            {
                throw BusinessRulesCode.SubCategoryNotExists.NewBusinessException();
            }

            TrimSubCategory(subCategoryInfo);
            subCategoryInfo.AdaptToSubCategory(subCategory);
            
            return subCategory;
        }

        public ProductPrice ModifyProductPrice(ProductPriceDto productPriceInfo)
        {
            var productPrice = UpdateProductPrice(productPriceInfo);
            ViewProductPrice.Modify(productPrice);
            context.ApplyChanges();

            return productPrice;
        }

        private ProductPrice UpdateProductPrice(ProductPriceDto productPriceInfo)
        {
            var productPrice = GetProductPrice(productPriceInfo.Id);
            if (productPrice == null)
            {
                throw BusinessRulesCode.ProductPriceNotExists.NewBusinessException();
            }

            TrimProductPrice(productPriceInfo);
            productPriceInfo.AdaptToProductPrice(productPrice);

            if ((productPrice.DisabledDate != null) && (productPrice.DisableNote == null))
            {
                throw BusinessRulesCode.ProductPriceDeactivateNote.NewBusinessException();
            }

            if (productPrice.DisableNote != null)
            {
                productPrice.DisabledDate = new DateTimeOffset(DateTime.UtcNow);
            }

            return productPrice;
        }

        public IEnumerable<Container> QueryContainer(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Container>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Container.Name, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Container, string>(pt => pt.Name, info.Direction));
                }
            }

            return ViewContainer.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetContainerSpecification(filterInfo));
        }

        public IEnumerable<Area> QueryArea(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Area>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Area.Name, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Area, string>(pt => pt.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Area.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Area, int>(pt => pt.Id, info.Direction));
                }
            }

            return
              ViewArea.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetAreaSpecification(filterInfo));
        }

        public IEnumerable<Category> QueryCategory(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Category>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Category.Area, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Category, string>(pt => pt.Area.Name, info.Direction));
                }
               
                if (info.Field.Equals(SortQuery.Category.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Category, int>(pt => pt.Id, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Category.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Category, string>(pt => pt.Name, info.Direction));
                }
            }

            return
              ViewCategory.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetCategorySpecification(filterInfo));
        }

        public IEnumerable<SubCategory> QuerySubCategory(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<SubCategory>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.SubCategory.Category, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<SubCategory, string>(pt => pt.Category.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.SubCategory.Area, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<SubCategory, string>(pt => pt.Category.Area.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.SubCategory.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<SubCategory, int>(pt => pt.Id, info.Direction));
                }

                if (info.Field.Equals(SortQuery.SubCategory.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<SubCategory, string>(pt => pt.Name, info.Direction));
                }
            }

            return
              ViewSubCategory.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetSubCategorySpecification(filterInfo));
        }

        public IEnumerable<ProductPrice> QueryProductPrice(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<ProductPrice>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.ProductPrice.Product, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<ProductPrice, string>(pt => pt.Product.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.ProductPrice.BuyMayorPrice, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<ProductPrice, string>(pt => pt.BuyMayorPrice.ToString(), info.Direction));
                }

                if (info.Field.Equals(SortQuery.ProductPrice.MayorGainPercent, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<ProductPrice, string>(pt => pt.MayorGainPercent.ToString(), info.Direction));
                }

                if (info.Field.Equals(SortQuery.ProductPrice.MinorGainPercent, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<ProductPrice, string>(pt => pt.MinorGainPercent.ToString(), info.Direction));
                }

                if (info.Field.Equals(SortQuery.ProductPrice.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<ProductPrice, int>(pt => pt.Id, info.Direction));
                }                
            }

            return
              ViewProductPrice.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetProductPriceSpecification(filterInfo));
        }

        public IEnumerable<Product> QueryProductByStock(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Product>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Product.Container, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, string>(pt => pt.Container.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Product.SubCategory, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, string>(pt => pt.SubCategory.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Product.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, int>(pt => pt.Id, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Product.Name, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, string>(pt => pt.Name, info.Direction));
                }
            }

            var products = ViewProduct.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetProductSpecification(filterInfo)).ToList();
            var stockSrv = new StockService(context);
            return products.FindAll(d => stockSrv.CheckStockByProduct(d.Id) > 0).ToList();           
        }

        public IEnumerable<Product> QueryProduct(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Product>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Product.Container, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, string>(pt => pt.Container.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Product.SubCategory, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, string>(pt => pt.SubCategory.Name, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Product.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, int>(pt => pt.Id, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Product.Name, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Product, string>(pt => pt.Name, info.Direction));
                }
            }

            return
              ViewProduct.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetProductSpecification(filterInfo));
        }

        public int CountProduct(FilterInfo filterInfo)
        {
            return ViewProduct.Count(GetProductSpecification(filterInfo));
        }

        private static Specification<Area> GetAreaSpecification(FilterInfo filterInfo)
        {
            Specification<Area> spec = new TrueSpecification<Area>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.Area.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecAreaByFullSearch(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<Category> GetCategorySpecification(FilterInfo filterInfo)
        {
            Specification<Category> spec = new TrueSpecification<Category>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.Category.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecCategoryByFullSearch(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<SubCategory> GetSubCategorySpecification(FilterInfo filterInfo)
        {
            Specification<SubCategory> spec = new TrueSpecification<SubCategory>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.SubCategory.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecSubCategoryByFullSearch(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<ProductPrice> GetProductPriceSpecification(FilterInfo filterInfo)
        {
            Specification<ProductPrice> spec = new TrueSpecification<ProductPrice>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.ProductPrice.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProductPriceByFullSearch(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<Product> GetProductSpecification(FilterInfo filterInfo)
        {
            Specification<Product> spec = new TrueSpecification<Product>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.Product.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProductByFullSearch(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.Product.Available, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProductByActive();
                }
            }

            return spec;
        }

        private static void TrimProduct(ProductDto productInfo)
        {
            productInfo.Name = FirstToCapital(productInfo.Name.Trim());
            productInfo.Description = FirstToCapital(productInfo.Description.Trim());
            productInfo.DisableNote = string.IsNullOrWhiteSpace(productInfo.DisableNote) ? null : productInfo.DisableNote.Trim();
        }

        private static void TrimContainer(ContainerDto containerInfo)
        {
            containerInfo.Name = FirstToCapital(containerInfo.Name.Trim());
            containerInfo.Description = FirstToCapital(containerInfo.Description.Trim());
        }

        private static void TrimArea(AreaDto areaInfo)
        {
            areaInfo.Name = FirstToCapital(areaInfo.Name.Trim());            
        }

        private static void TrimCategory(CategoryDto categoryInfo)
        {
            categoryInfo.Name = FirstToCapital(categoryInfo.Name.Trim());                        
        }

        private static void TrimSubCategory(SubCategoryDto subCategoryInfo)
        {
            subCategoryInfo.Name = FirstToCapital(subCategoryInfo.Name.Trim());                        
        }

        private static void TrimProductPrice(ProductPriceDto productPriceInfo)
        {                     
            productPriceInfo.DisableNote = string.IsNullOrWhiteSpace(productPriceInfo.DisableNote) ? null : productPriceInfo.DisableNote.Trim();
        }

        private static string FirstToCapital(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return data;
            }

            var chars = data.ToCharArray();
            chars[0] = chars[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant().ToCharArray()[0];
            return new string(chars);
        }

        private static Specification<Product> SpecProductByFullSearch(string value)
        {
            int valueId;
            Specification<Product> result = new DirectSpecification<Product>(e => e.Name.ToLower().Contains(value.ToLower()));
            result |= new DirectSpecification<Product>(e => e.SubCategory.Name.Contains(value.ToLower()));
            result |= new DirectSpecification<Product>(e => e.Container.Name.Contains(value.ToLower()));
            if (int.TryParse(value, out valueId))
            {
                result |= new DirectSpecification<Product>(e => e.Id.Equals(valueId));
            }

            return result;
        }

        private static Specification<Product> SpecProductByActive()
        {
            return new DirectSpecification<Product>(e => e.ForSale);
        }

        private static Specification<Area> SpecAreaByFullSearch(string value)
        {
            return new DirectSpecification<Area>(p => p.Name.ToLower().Contains(value.ToLower()));
        }

        private static Specification<Category> SpecCategoryByFullSearch(string value)
        {
            return new OrSpecification<Category>(
              new DirectSpecification<Category>(p => p.Name.ToLower().Contains(value.ToLower())),
              new DirectSpecification<Category>(p => p.Area.Name.ToLower().Contains(value.ToLower())));
        }

        private static Specification<SubCategory> SpecSubCategoryByFullSearch(string value)
        {
            return new OrSpecification<SubCategory>(
              new DirectSpecification<SubCategory>(p => p.Name.ToLower().Contains(value.ToLower())),
              new DirectSpecification<SubCategory>(p => p.Category.Name.ToLower().Contains(value.ToLower())));
        }

        private static Specification<ProductPrice> SpecProductPriceByFullSearch(string value)
        {
            return new DirectSpecification<ProductPrice>(p => p.Product.Name.ToLower().Contains(value.ToLower()));
        }

        private static Specification<Container> GetContainerSpecification(FilterInfo filterInfo)
        {
            Specification<Container> spec = new TrueSpecification<Container>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.Container.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecContainerByFullSearch(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<Container> SpecContainerByFullSearch(string value)
        {
            return new OrSpecification<Container>(
              new DirectSpecification<Container>(p => p.Name.ToLower().Contains(value.ToLower())),
              new DirectSpecification<Container>(p => p.Description.ToLower().Contains(value.ToLower())));
        }
    }
}