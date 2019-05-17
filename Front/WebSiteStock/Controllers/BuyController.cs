namespace SAC.Stock.Front.Controllers
{
    using SAC.Seed.Dependency;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Front.Infrastructure;
    using SAC.Stock.Front.Infrastructure.Authorize;
    using SAC.Stock.Front.Models;
    using SAC.Stock.Front.Models.Bill;
    using SAC.Stock.Front.Models.BranchOffice;
    using SAC.Stock.Front.Models.Buy;
    using SAC.Stock.Front.Models.Product;
    using SAC.Stock.Front.Models.Provinder;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.BillContext;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.BuyContext;
    using SAC.Stock.Service.ProductContext;
    using SAC.Stock.Service.ProviderContext;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;    
    using System.Web.Http;
    using System.Web.Mvc;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;    

    [System.Web.Http.Authorize]
    public class BuyController : Controller
    {
        private IBuyApplicationService BuyApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBuyApplicationService>();
            }
        }

        private IProviderApplicationService ProviderApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IProviderApplicationService>();
            }
        }

        private IProductApplicationService ProductApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IProductApplicationService>();
            }
        }

        private IBranchOfficeApplicationService BranchOfficeApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBranchOfficeApplicationService>();
            }
        }

        private IBillApplicationService BillApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBillApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public JsonResult SaveBuy(BuyDpo buyDpo)
        {
            try
            {
                BuyDto buy = null;
                if (buyDpo.Id == 0)
                {
                    buy = BuyApplicationSvc.AddBuy(buyDpo.AdaptToBuyDto());                    
                }
                else
                {
                    buy = BuyApplicationSvc.ModifyBuy(buyDpo.AdaptToBuyDto());
                }

                buyDpo.Id = buy.Id;
                return Json(buyDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.BuyWithoutBranchOffice.Code,
              BusinessRulesCode.BuyWithoutBranchOfficeStaff.Code,
              BusinessRulesCode.BuyWithoutProvider.Code,
              BusinessRulesCode.BuyNotExists.Code,
              BusinessRulesCode.TransactionWithoutBill.Code,
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

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public JsonResult GetBuys(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var buys = BuyApplicationSvc.QueryBuy(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(buys.Select(p => p.AdaptToBuyDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public JsonResult GetProviders(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var providers = ProviderApplicationSvc.QueryProvider(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(providers.Select(p => p.AdaptToProviderDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public JsonResult GetEmployees(QueryInfo queryInfo)
        {
            try
            {
                var user = AppUser.Get();
                var filter = new List<FilterInfo>();
                if (!user.Roles.Any(c => c.Equals(CodeConst.Role.StockSuperAdmin.Code)))
                {
                    var branchOfficeId = user.Attributes.FirstOrDefault(c => c.AttributeCode.Equals(CodeConst.Attribute.BranchOfficeId.Code)).Value;
                    filter.Add(new FilterInfo
                    {
                        Spec = SpecFilter.BranchOfficeStaff.BranchOfficeId,
                        Value = branchOfficeId
                    });
                }

                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var employees = BranchOfficeApplicationSvc.QueryBranchOfficeStaff(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter.FirstOrDefault());

                return Json(employees.Select(p => p.AdaptToBranchOfficeStaffDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public JsonResult GetBranchOffices(QueryInfo queryInfo)
        {
            try
            {
                var user = AppUser.Get();
                var filter = new List<FilterInfo>();
                if (!user.Roles.Any(c => c.Equals(CodeConst.Role.StockSuperAdmin.Code)))
                {
                    return null;
                }

                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var branchOffices = BranchOfficeApplicationSvc.QueryBranchOffice(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(branchOffices.Select(p => p.AdaptToBranchOfficeDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Buys)]
        public JsonResult GetBills()
        {
            try
            {
                return Json(BillApplicationSvc.QueryBill().Select(p => p.AdaptToBillDpo()));
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