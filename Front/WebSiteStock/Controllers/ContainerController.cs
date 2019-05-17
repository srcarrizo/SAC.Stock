namespace SAC.Stock.Front.Controllers
{
    using Seed.Dependency;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using Infrastructure;
    using Infrastructure.Authorize;
    using Models;
    using WebSiteStock.Models;
    using Service.ProductContext;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;
    using SAC.Stock.Front.Models.Product;

    [System.Web.Http.Authorize]
    public class ContainerController : Controller
    {
        private IProductApplicationService ProductApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IProductApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Containers)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Containers)]
        [System.Web.Http.HttpPost]
        public JsonResult SaveContainer(ContainerDpo containerDpo)
        {
            try
            {
                ContainerDto container;
                if (containerDpo.Id == 0)
                {
                    container = ProductApplicationSvc.AddContainer(containerDpo.AdaptContainerToDto());
                }
                else
                {
                    container = ProductApplicationSvc.ModifyContainer(containerDpo.AdaptContainerToDto());
                }

                containerDpo.Id = container.Id;
                return Json(containerDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
              BusinessRulesCode.ContainerNotExists.Code,              
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

        [MvcAuthorizeOption(MenuDefinition.Code.Containers)]
        public JsonResult GetContainers(QueryInfo queryInfo)
        {
            try
            {
                var containers = ProductApplicationSvc.QueryContainer(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    queryInfo.FilterInfo);

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