using SAC.Stock.Front.Models.Customer;

namespace SAC.Stock.Front.Controllers
{
    using Models;
    using Seed.Dependency;
    using Service.ProviderContext;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using Models.Provinder;
    using WebSiteStock.Models;
    using Infrastructure.Authorize;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using System.Web.Http;
    using Infrastructure;
    using System.Net.Http;
    using Models.Shared;
    using Service.LocationContext;
    using Service.SupportContext;

    [System.Web.Http.Authorize]
    public class ProviderController : Controller
    {
        private IProviderApplicationService ProviderApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IProviderApplicationService>();
            }
        }

        private ISupportApplicationService SupportApplicationService
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<ISupportApplicationService>();
            }
        }

        private ILocationApplicationService LocationApplicationService
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<ILocationApplicationService>();
            }
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Providers)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        [System.Web.Http.HttpPost]
        public JsonResult GetChildLocations(LocationDpo location)
        {
            try
            {
                return Json(LocationApplicationService.QueryChildrenLocation(location.Id, true));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        [System.Web.Http.HttpPost]
        public JsonResult GetUidCode(LocationDpo location)
        {
            try
            {
                return Json(CodeConst.UidType.Values());
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

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        [System.Web.Http.HttpPost]
        public JsonResult GetTelcoList()
        {
            try
            {
                return Json(SupportApplicationService.QueryTelco());
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

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        [System.Web.Http.HttpPost]
        public JsonResult GetCountryList()
        {
            try
            {
                return Json(LocationApplicationService.QueryLocationCountry());
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

        [MvcAuthorizeOption(MenuDefinition.Code.Providers)]
        [System.Web.Http.HttpPost]
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

        public JsonResult SaveProvider(ProviderDpo providerDpo)
        {
            try
            {
                var provider = ProviderApplicationSvc.AddProvider(providerDpo.AdaptToProviderDto());
                var result = new
                {
                    provider.Id,
                    provider.CreatedDate,
                    provider.DeactivateDate,
                    provider.DeativateNote,
                    provider.Name,                    
                    provider.Person.Address,
                    provider.Person.BirthDate,
                    provider.Person.Email,
                    provider.Person.FirstName,
                    provider.Person.LastName,
                    provider.Person.GenderCode,
                    provider.Person.Phones,
                    provider.Person.UidCode,
                    provider.Person.UidSerie,
                    UidCodeSerie = provider.Person.UidCode + ": " + provider.Person.UidSerie,
                };

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
                        BusinessRulesCode.CustomerExists.Code,
                        BusinessRulesCode.PersonNotExists.Code,
                        BusinessRulesCode.InvalidDateOfBirth.Code,
                        BusinessRulesCode.LocationNotExists.Code,
                        BusinessRulesCode.LocationNotCity.Code,
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