namespace SAC.Stock.Front.Controllers
{
    using SAC.Seed.Dependency;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Front.Infrastructure;
    using SAC.Stock.Front.Infrastructure.Authorize;
    using SAC.Stock.Front.Models.Bill;
    using SAC.Stock.Front.Models.Box;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.BillContext;
    using SAC.Stock.Service.BoxContext;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;

    [System.Web.Http.Authorize]
    public class BoxController : Controller
    {
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

        [MvcAuthorizeOption(MenuDefinition.Code.Box)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Box)]
        [System.Web.Http.HttpPost]
        public JsonResult GetBox()
        {
            try
            {                
                var boxes = BoxApplicationSvc.GetLatestBoxWithSalesBuysTransaction();
                var dpo = new
                {
                    Box = boxes.Box == null ? new BoxDpo
                    {
                        OpenDate = null,
                        OpenNote = "Inicio de Caja",
                        OpeningClosing = false,
                        Detail = new List<BoxDetailDpo>()
                    } : boxes.Box.AdaptToBoxDpo(),
                    boxes.UnprocessedBuys,
                    boxes.UnprocessedSales,
                    boxes.UnprocessedTransactions
                };

                return Json(dpo);
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

        [MvcAuthorizeOption(MenuDefinition.Code.Box)]
        public JsonResult SaveBox(BoxDpo boxDpo)
        {
            try
            {
                BoxApplicationSvc.OpenCloseBox(boxDpo.AdaptToBoxDto());
                var boxes = BoxApplicationSvc.GetLatestBoxWithSalesBuysTransaction();
                var dpo = new
                {
                    Box = boxes.Box.AdaptToBoxDpo(),
                    boxes.UnprocessedBuys,
                    boxes.UnprocessedSales,
                    boxes.UnprocessedTransactions
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
              BusinessRulesCode.BoxOpeningWithoutOpenDate.Code,
              BusinessRulesCode.BoxOpeningWithoutOpenNote.Code,
              BusinessRulesCode.BoxClosingingWithoutCloseDate.Code,
              BusinessRulesCode.BoxClosingWithoutClosingNote.Code,              
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

        [MvcAuthorizeOption(MenuDefinition.Code.Box)]
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