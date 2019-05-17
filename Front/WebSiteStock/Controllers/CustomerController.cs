namespace SAC.Stock.Front.Controllers
{
    using Seed.Dependency;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using Infrastructure;
    using Infrastructure.Authorize;
    using Models;
    using Models.Customer;
    using WebSiteStock.Models;
    using Service.CustomerContext;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;
    using Service.SupportContext;
    using Models.Shared;
    using Service.LocationContext;


    [System.Web.Http.Authorize]
    public class CustomerController : Controller
    {
        private ICustomerApplicationService CustomerApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<ICustomerApplicationService>();
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

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        [System.Web.Http.HttpPost]
        public JsonResult GetCustomer(QueryInfo queryInfo)
        {
            try
            {
                var filter = new List<FilterInfo>();
                if (queryInfo.FilterInfo != null)
                {
                    filter.Add(queryInfo.FilterInfo);
                }

                var customers = CustomerApplicationSvc.QueryCustomer(
                    queryInfo.PageIndex,
                    queryInfo.PageSize,
                    queryInfo.SortInfo,
                    filter);

                return Json(customers.Select(c => c.AdaptToCustomerDpo()));
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

        [MvcAuthorizeOption(MenuDefinition.Code.Customers)]
        [System.Web.Http.HttpPost]

        public JsonResult SaveCustomer(CustomerDpo customerDpo)
        {
            try
            {
                var customer = CustomerApplicationSvc.AddCustomer(customerDpo.AdaptToCustomerDto());
                var result = new
                {
                    customer.Id,
                    customer.CreatedDate,
                    customer.DeactivateDate,
                    customer.DeativateNote,
                    customer.Name,
                    customer.UserId,
                    customer.Person.Address,
                    customer.Person.BirthDate,
                    customer.Person.Email,
                    customer.Person.FirstName,
                    customer.Person.LastName,
                    customer.Person.GenderCode,
                    customer.Person.Phones,
                    customer.Person.UidCode,
                    customer.Person.UidSerie,
                    UidCodeSerie = customer.Person.UidCode + ": " + customer.Person.UidSerie,
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