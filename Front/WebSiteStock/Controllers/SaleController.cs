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
    using SAC.Stock.Front.Models.Customer;
    using SAC.Stock.Front.Models.Product;
    using SAC.Stock.Front.Models.Sale;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.BillContext;
    using SAC.Stock.Service.BoxContext;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.CustomerContext;
    using SAC.Stock.Service.ProductContext;    
    using SAC.Stock.Service.SaleContext;
    using SAC.Stock.Service.StockContext;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;    
    using System.Web.Http;
    using System.Web.Mvc;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;

    [System.Web.Mvc.Authorize]
    public class SaleController : Controller
    {
        private ISaleApplicationService SaleApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<ISaleApplicationService>();
            }
        }

        private ICustomerApplicationService CustomerApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<ICustomerApplicationService>();
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

        private IBoxApplicationService BoxApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBoxApplicationService>();
            }
        }

        private IBillApplicationService BillApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBillApplicationService>();
            }
        }

        private IStockApplicationService StockApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IStockApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult SaveSale(SaleDpo SaleDpo)
        {
            try
            {
                SaleDto sale;
                if (SaleDpo.Id == 0)
                {
                    sale = SaleApplicationSvc.AddSale(SaleDpo.AdaptToSaleDto());
                }
                else
                {
                     sale = SaleApplicationSvc.ModifySale(SaleDpo.AdaptToSaleDto());
                }

                SaleDpo.Id = sale.Id;
                foreach (var detailDpo in SaleDpo.Detail)
                {
                    foreach (var detailDto in sale.Detail)
                    {
                        if (detailDpo.Product.Id == detailDto.ProductId)
                        {
                            detailDpo.Id = detailDto.Id;
                        }
                    }
                }

                return Json(SaleDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.SaleWithoutBranchOffice.Code,
              BusinessRulesCode.SaleWithoutBranchOfficeStaff.Code,
              BusinessRulesCode.SaleWithoutCustomer.Code,
              BusinessRulesCode.SaleNotExists.Code,
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult DeletePreSale(SaleDpo SaleDpo)
        {
            try
            {
                var result = SaleApplicationSvc.DeletePreSale(SaleDpo.AdaptToSaleDto());                             
                return Json(result);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.NotPreSale.Code
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult CompleteSale(SaleDpo SaleDpo)
        {
            try
            {
                var result = SaleApplicationSvc.CompleteSale(SaleDpo.AdaptToSaleDto());
                return Json(result.AdaptToSaleDpo());
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.SaleNotExists.Code
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult GetSales(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var Sales = SaleApplicationSvc.QuerySale(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(Sales.Select(p => p.AdaptToSaleDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult CheckOpenBox()
        {
            try
            {
                var box = BoxApplicationSvc.GetLatestBox();                
                return Json(box != null ? box.OpenDate.HasValue : false);
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult GetCustomers(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var providers = CustomerApplicationSvc.QueryCustomer(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(providers.Select(p => p.AdaptToCustomerDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public int CheckProductStock(int productId)
        {
            try
            {
                return StockApplicationSvc.CheckStockByProduct(productId);
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult GetStock()
        {
            try
            {
                return Json(StockApplicationSvc.GetCurrentStock());
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult GetProducts(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var products = ProductApplicationSvc.QueryProductByStock(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter.FirstOrDefault());

                return Json(products.Select(p => p.AdaptToProductDpo()));
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.LowStock.Code,
              BusinessRulesCode.NoStock.Code
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
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