using SAC.Seed.Dependency;
using SAC.Seed.NLayer.ExceptionHandling;
using SAC.Stock.Code;
using SAC.Stock.Front.Infrastructure;
using SAC.Stock.Front.Infrastructure.Authorize;
using SAC.Stock.Front.Models;
using SAC.Stock.Front.Models.Product;
using SAC.Stock.Front.WebSiteStock.Models;
using SAC.Stock.Service.ProductContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using FilterInfo = SAC.Seed.NLayer.Data.FilterInfo;

namespace SAC.Stock.Front.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ProductController : Controller
    {
        private IProductApplicationService ProductApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IProductApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Products)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Products)]
        [System.Web.Http.HttpPost]
        public JsonResult GetProducts(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var products = ProductApplicationSvc.QueryProduct(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter.FirstOrDefault());

                return Json(products.Select(p => p.AdaptToProductDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Products)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Products)]
        public JsonResult SaveProduct(ProductDpo productDpo)
        {
            try
            {
                ProductDto product;
                productDpo.CurrentProductPrice.UserId = AppUser.Get().Id;
                productDpo.CurrentProductPrice.CreatedDate = DateTimeOffset.Now;

                if (productDpo.Id == 0)
                {
                    product = ProductApplicationSvc.AddProduct(productDpo.AdaptToProductDto());
                }
                else
                {
                    product = ProductApplicationSvc.ModifyProduct(productDpo.AdaptToProductDto());
                }

                productDpo.Id = product.Id;
                productDpo.CurrentProductPrice.Id = product.ProductPrices.First().Id;

                return Json(productDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.ProductNotExists.Code,
              BusinessRulesCode.ProductDeactivateNote.Code,
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

        [MvcAuthorizeOption(MenuDefinition.Code.Products)]
        public JsonResult GetContainers(QueryInfo queryInfo)
        {
            try
            {
                var containers = ProductApplicationSvc.QueryContainer(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    queryInfo.FilterInfo).Where(c => c.ParentContainer != null);

                return Json(containers);
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