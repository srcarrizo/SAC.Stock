using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SAC.Seed.Dependency;
using SAC.Seed.NLayer.ExceptionHandling;
using SAC.Stock.Code;
using SAC.Stock.Front.Infrastructure;
using SAC.Stock.Front.Infrastructure.Authorize;
using SAC.Stock.Front.Models;
using SAC.Stock.Front.Models.Bill;
using SAC.Stock.Front.Models.BranchOffice;
using SAC.Stock.Front.Models.Budget;
using SAC.Stock.Front.Models.Customer;
using SAC.Stock.Front.Models.Product;
using SAC.Stock.Front.WebSiteStock.Models;
using SAC.Stock.Service.BillContext;
using SAC.Stock.Service.BoxContext;
using SAC.Stock.Service.BranchOfficeContext;
using SAC.Stock.Service.BudgetContext;
using SAC.Stock.Service.CustomerContext;
using SAC.Stock.Service.ProductContext;
using SAC.Stock.Service.StockContext;
using FilterInfo = SAC.Seed.NLayer.Data.FilterInfo;

namespace SAC.Stock.Front.Controllers
{
    public class BudgetController : Controller
    {
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

        private IBillApplicationService BillApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBillApplicationService>();
            }
        }

        private IBudgetApplicationService BudgetApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBudgetApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
        public JsonResult GetBudgets(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var providers = BudgetApplicationSvc.QueryBudget(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(providers.Select(p => new
                {
                    p.Id,
                    p.BudgetDate,
                    Name = p.Customer == null ? p.NonCustomerName : p.Customer.Name,
                    p.Customer,
                    p.BranchOffice,
                    p.BranchOfficeStaff,
                    p.PaymentTypeCode,
                    p.NonCustomerName,
                    p.Detail                    
                }).ToList());
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
        public JsonResult SaveBudget(BudgetDpo budgetDpo)
        {
            try
            {
                //BudgetDto budget;
                //if (BudgetDpo.Id == 0)
                //{
                   var budget = BudgetApplicationSvc.AddBudget(budgetDpo.AdaptToBudgetDto());
                //}
                //else
                //{
                //    budget = BudgetApplicationSvc.ModifySale(BudgetDpo.AdaptToSaleDto());
                //}

                budgetDpo.Id = budget.Id;
                budgetDpo.Name = budgetDpo.Customer == null ? budgetDpo.NonCustomerName : budgetDpo.Customer.Name;                
                return Json(budgetDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.BudgetWithoutBranchOffice.Code,
              BusinessRulesCode.BudgetWithoutBranchOfficeStaff.Code,
              BusinessRulesCode.BudgetWithoutCustomer.Code,
              BusinessRulesCode.BudgetNotExists.Code,              
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

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Budget)]
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