namespace SAC.Stock.Front.Controllers
{
    using SAC.Seed.Dependency;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Front.Infrastructure;
    using SAC.Stock.Front.Infrastructure.Authorize;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.StockContext;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;

    [System.Web.Mvc.Authorize]
    public class StockController : Controller
    {
        private IStockApplicationService StockApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IStockApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Stock)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Stock)]
        public JsonResult SaveStock()
        {
            try
            {                
                var newStock = StockApplicationSvc.CreateStock();
                var stock = StockApplicationSvc.GetCurrentStock();
                stock.ProcsessedBuys = newStock.ProcsessedBuys;
                stock.ProcsessedSales = newStock.ProcsessedSales;
                stock.PreSales = newStock.PreSales;

                return Json(stock);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
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

        [System.Web.Http.HttpPost]
        [MvcAuthorizeOption(MenuDefinition.Code.Stock)]
        public JsonResult GetLatestStock()
        {
            try
            {
                var stock = StockApplicationSvc.GetCurrentStock();                
                return Json(stock);
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