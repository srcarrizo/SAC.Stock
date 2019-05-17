namespace SAC.Stock.Front.Controllers
{
    using Seed.Dependency;
    using Infrastructure.Authorize;
    using Models;
    using Models.BranchOffice;
    using WebSiteStock.Models;
    using Service.BranchOfficeContext;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;
    using SAC.Stock.Service.SupportContext;
    using SAC.Stock.Service.LocationContext;
    using SAC.Stock.Front.Models.Shared;
    using System;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Front.Infrastructure;
    using SAC.Stock.Code;
    using System.Web.Http;
    using System.Net.Http;

    [System.Web.Mvc.Authorize]
    public class BranchOfficeController : Controller
    {
        private IBranchOfficeApplicationService BranchOfficeApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IBranchOfficeApplicationService>();
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

        [MvcAuthorizeOption(MenuDefinition.Code.Branchoffice)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Branchoffice)]
        [System.Web.Mvc.HttpPost]
        public JsonResult GetBranchOffices(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
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

        [MvcAuthorizeOption(MenuDefinition.Code.UserManager)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.UserManager)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.UserManager)]
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

        [MvcAuthorizeOption(MenuDefinition.Code.Sales)]
        public JsonResult SaveBranchOffice(BranchOfficeDpo branchOfficeDpo)
        {
            try
            {
                BranchOfficeDto branchOffice;
                if (branchOfficeDpo.Id == Guid.Empty)
                {
                    branchOffice = BranchOfficeApplicationSvc.AddBranchOffice(branchOfficeDpo.AdaptToBranchOfficeDto());
                }
                else
                {
                    branchOffice = BranchOfficeApplicationSvc.ModifyBranchOffice(branchOfficeDpo.AdaptToBranchOfficeDto());
                }

                branchOfficeDpo.Id = branchOffice.Id;
                return Json(branchOfficeDpo);
            }
            catch (BusinessRulesException ex)
            {
                var message = Request.RequestContext.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var error = HttpErrorCode.ErrorDataFromReport(
                  ex.Report,
                  message,
                  new[]
                    {
                      BusinessRulesCode.DuplicateCode.Code,
                      BusinessRulesCode.BranchOfficeByIdNotExists.Code,
                      BusinessRulesCode.BranchOfficeShouldNotChangeActivate.Code,
                      BusinessRulesCode.BranchOfficeDeactivateWithoutActivate.Code,
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