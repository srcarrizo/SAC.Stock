namespace SAC.Stock.Front.Controllers
{
    using SAC.Seed.Dependency;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Front.Infrastructure;
    using SAC.Stock.Front.Infrastructure.Authorize;
    using SAC.Stock.Front.Models;
    using SAC.Stock.Front.Models.Product;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.ProductContext;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;

    [System.Web.Http.Authorize]
    public class AreaCategoryController : Controller
    {
        private IProductApplicationService ProductApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IProductApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        public JsonResult GetAreas(QueryInfo queryInfo)
        {
            try
            {  
                var areas = ProductApplicationSvc.QueryArea(queryInfo.PageIndex,
                queryInfo.PageSize,
                queryInfo.SortInfo,
                queryInfo.FilterInfo);

                return Json(areas);
            }
            catch (DistributedServiceException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                if (!ex.Code.Equals(BusinessRulesCode.FailedConnectDatabase.Code))
                {
                    throw new HttpResponseException(HttpErrorCode.FailedConnectDatabase.CreateErrorResponse(message));
                }

                throw;
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        public JsonResult GetCategories(QueryInfo queryInfo)
        {
            try
            {
                return Json(ProductApplicationSvc.QueryCategory(queryInfo.PageIndex,
                queryInfo.PageSize,
                queryInfo.SortInfo,
                queryInfo.FilterInfo));
            }
            catch (DistributedServiceException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                if (!ex.Code.Equals(BusinessRulesCode.FailedConnectDatabase.Code))
                {
                    throw new HttpResponseException(HttpErrorCode.FailedConnectDatabase.CreateErrorResponse(message));
                }

                throw;
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        public JsonResult GetSubCategories(QueryInfo queryInfo)
        {
            try
            {
                return Json(ProductApplicationSvc.QuerySubCategory(queryInfo.PageIndex,
                queryInfo.PageSize,
                queryInfo.SortInfo,
                queryInfo.FilterInfo));
            }
            catch (DistributedServiceException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                if (!ex.Code.Equals(BusinessRulesCode.FailedConnectDatabase.Code))
                {
                    throw new HttpResponseException(HttpErrorCode.FailedConnectDatabase.CreateErrorResponse(message));
                }

                throw;
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        [System.Web.Http.HttpPost]
        public JsonResult SaveArea(AreaDpo areaDpo)
        {
            try
            {
                AreaDto area;
                if (areaDpo.Id == 0)
                {
                    area = ProductApplicationSvc.AddArea(areaDpo.AdaptAreaToDto());
                }
                else
                {
                    area = ProductApplicationSvc.ModifyArea(areaDpo.AdaptAreaToDto());
                }

                areaDpo.Id = area.Id;               
                return Json(areaDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.AreaNotExists.Code,              
              BusinessRulesCode.NoUserLoggedIn.Code
                    });
                if (error != null)
                {
                    throw new HttpResponseException(error);
                }

                throw;
            }
            catch (DistributedServiceException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                if (!ex.Code.Equals(BusinessRulesCode.FailedConnectDatabase.Code))
                {
                    throw new HttpResponseException(HttpErrorCode.FailedConnectDatabase.CreateErrorResponse(message));
                }

                throw;
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        [System.Web.Http.HttpPost]
        public JsonResult SaveCategory(CategoryDpo categoryDpo)
        {
            try
            {
                CategoryDto category;
                if (categoryDpo.Id == 0)
                {
                    category = ProductApplicationSvc.AddCategory(categoryDpo.AdaptCategoryToDto());
                }
                else
                {
                    category = ProductApplicationSvc.ModifyCategory(categoryDpo.AdaptCategoryToDto());
                }

                categoryDpo.Id = category.Id;
                return Json(categoryDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.CategoryNotExists.Code,              
              BusinessRulesCode.NoUserLoggedIn.Code
                    });
                if (error != null)
                {
                    throw new HttpResponseException(error);
                }

                throw;
            }
            catch (DistributedServiceException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                if (!ex.Code.Equals(BusinessRulesCode.FailedConnectDatabase.Code))
                {
                    throw new HttpResponseException(HttpErrorCode.FailedConnectDatabase.CreateErrorResponse(message));
                }

                throw;
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.AreaCategory)]
        [System.Web.Http.HttpPost]
        public JsonResult SaveSubCategory(SubCategoryDpo subCategoryDpo)
        {
            try
            {
                SubCategoryDto subCategory;
                if (subCategoryDpo.Id == 0)
                {
                    subCategory = ProductApplicationSvc.AddSubCategory(subCategoryDpo.AdaptToSubCategoryDto());
                }
                else
                {
                    subCategory = ProductApplicationSvc.ModifySubCategory(subCategoryDpo.AdaptToSubCategoryDto());
                }

                subCategoryDpo.Id = subCategory.Id;
                return Json(subCategoryDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.SubCategoryNotExists.Code,              
              BusinessRulesCode.NoUserLoggedIn.Code
                    });
                if (error != null)
                {
                    throw new HttpResponseException(error);
                }

                throw;
            }
            catch (DistributedServiceException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                if (!ex.Code.Equals(BusinessRulesCode.FailedConnectDatabase.Code))
                {
                    throw new HttpResponseException(HttpErrorCode.FailedConnectDatabase.CreateErrorResponse(message));
                }

                throw;
            }
        }
    }
}