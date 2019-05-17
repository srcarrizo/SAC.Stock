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
    using SAC.Stock.Front.Models.Transaction;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.BillContext;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.TransactionContext;    
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;    
    using System.Web.Http;
    using System.Web.Mvc;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;

    [System.Web.Mvc.Authorize]
    public class TransactionController : Controller
    {
        private ITransactionApplicationService TransactionApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<ITransactionApplicationService>();
            }
        }

        private IBillApplicationService BillApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBillApplicationService>();
            }
        }

        private IBranchOfficeApplicationService BranchOfficeApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBranchOfficeApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
        [System.Web.Http.HttpPost]
        public JsonResult GetTransaction()
        {
            try
            {
                var transaction = TransactionApplicationSvc.QueryTransactionNotBoxedWithTotals();
                var transactions = new
                {
                    Transactions = transaction.Transactions.Select(t => t.AdaptToTransactionDpo()).ToList(),
                    transaction.TotalIn,
                    transaction.TotalOut
                };
                return Json(transactions);
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

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
        [System.Web.Http.HttpPost]
        public JsonResult GetTransactionHistory()
        {
            try
            {
                var transactions = TransactionApplicationSvc.QueryAllTransaction();                
                return Json(transactions);
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

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
        public JsonResult SaveTransaction(TransactionDpo transactionDpo)
        {
            try
            {
                var result = TransactionApplicationSvc.AddTransaction(transactionDpo.AdaptToTransactionDto());
                transactionDpo.Id = result.Id;

                var total = from detail in transactionDpo.Detail
                            select new { Total = detail.Bill.BillUnitType.IsDecimal ? ((decimal)detail.Amount * detail.Bill.Value) / 100 : (decimal)detail.Amount * detail.Bill.Value };

                var dpo = new
                {
                    Transaction = transactionDpo,
                    TotalIn = transactionDpo.TransactionTypeInOut ? total.Sum(d => d.Total) : 0,
                    TotalOut = !transactionDpo.TransactionTypeInOut ? total.Sum(d => d.Total) : 0,
                };                

               return Json(dpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.TransactionWithoutBranchOffice.Code,
              BusinessRulesCode.TransactionWithoutBranchOfficeStaff.Code,
              BusinessRulesCode.TransactionDoesNotExist.Code,              
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

        [MvcAuthorizeOption(MenuDefinition.Code.Transaction)]
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
    }
}