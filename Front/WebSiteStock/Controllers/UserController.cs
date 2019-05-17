namespace SAC.Stock.Front.Controllers
{
    using Membership.Service.UserManagement;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Front.Infrastructure;
    using SAC.Stock.Front.Infrastructure.Authorize;
    using SAC.Stock.Front.Models;
    using SAC.Stock.Front.Models.BranchOffice;
    using SAC.Stock.Front.Models.Shared;
    using SAC.Stock.Front.WebSiteStock.Models;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.LocationContext;
    using SAC.Stock.Service.SupportContext;
    using Seed.Dependency;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;
    using FilterInfo = Seed.NLayer.Data.FilterInfo;

    [System.Web.Http.Authorize]
    public class UserController : Controller
    {
        private IUserManagementApplicationService UserManagementApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IUserManagementApplicationService>();
            }
        }

        private IBranchOfficeApplicationService BranchOfficeApplicationService
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

        [MvcAuthorizeOption(MenuDefinition.Code.UserManager)]
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

                var branchOffices = BranchOfficeApplicationService.QueryBranchOffice(
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
        public JsonResult SaveStaff(BranchOfficeStaffDpo staffDpo)
        {
            try
            {
                var staff = BranchOfficeApplicationService.AddBranchOfficeStaff(staffDpo.AdaptToBranchOfficeStaffSaveDto());
                var result = new
                {
                    staff.Id,
                    staff.Staff,
                    staff.StaffRoleCode,
                    staff.UserId,
                    staff.BranchOffice,
                    staff.CreatedDate,
                    staff.DeactivatedDate,
                    staff.DeactivateNote,
                    UserManagementApplicationSvc.GetUser(staff.UserId).UserName,
                    StaffRoleName = CodeConst.StaffRole.Values().Where(v => v.Code.Equals(staff.StaffRoleCode)).FirstOrDefault().Name
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
              BusinessRulesCode.BranchOfficeStaffPrevious.Code,
              BusinessRulesCode.UserExists.Code,
              BusinessRulesCode.PersonNotExists.Code,
              BusinessRulesCode.InvalidDateOfBirth.Code,
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

        [MvcAuthorizeOption(MenuDefinition.Code.UserManager)]
        public ActionResult Index()
        {
            try
            {
                var users = UserManagementApplicationSvc.GetUser(AppUser.Get().UserName);
                return View();
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
        public JsonResult GetUsers(QueryInfo queryInfo)
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

                var employees = BranchOfficeApplicationService.QueryBranchOfficeStaff(
                      queryInfo.PageIndex,
                      queryInfo.PageSize,
                      queryInfo.SortInfo,
                      filter.FirstOrDefault()).Select(e => new
                      {
                          e.Id,
                          e.Staff,
                          e.StaffRoleCode,
                          e.UserId,
                          e.BranchOffice,
                          e.CreatedDate,
                          e.DeactivatedDate,
                          e.DeactivateNote,
                          UserManagementApplicationSvc.GetUser(e.UserId).UserName,
                          StaffRoleName = CodeConst.StaffRole.Values().Where(v => v.Code.Equals(e.StaffRoleCode)).FirstOrDefault().Name
                      }).ToList();

                return Json(employees);
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
        public JsonResult GetRolesComposition()
        {
            try
            {
                var roles = RolesComposition.RolesDefinitions.Where(r => r.ScopeItem.Code.Equals(CodeConst.Scope.BranchOffice.Code)).ToList();
                var result = roles.First().StaffRoles.Select(r =>
                new
                {
                    r.Hierarchy,
                    r.StaffRoleItem,
                    r.Roles
                }).ToList().OrderBy(r => r.Hierarchy);

                return Json(result);
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
    }
}