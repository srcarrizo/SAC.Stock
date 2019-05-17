namespace SAC.Stock.Front.Controllers
{    
    using Membership.Service.UserAccess;
    using Membership.Service.UserManagement;
    using Seed.Dependency;
    using WebSiteStock.Models;
    using System.Web.Mvc;    
    using Infrastucture;
    using Code;
    using System.Collections.Generic;
    using Infrastructure;
    using Seed.NLayer.ExceptionHandling;
    using Seed.ExceptionHandling;
    using System.Linq;    
    using System;
    using Infrastructure.Diagnostic;    
    using WebSiteStock.Models.Shared;

    [Authorize]
    public class AccountController : Controller
    {
        private IUserAccessApplicationService UserAccessApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IUserAccessApplicationService>();
            }
        }

        private IUserManagementApplicationService UserManagementApplicationSvc
        {
            get
            {
                return DiContainerFactory.DiContainer().BeginScope().Resolve<IUserManagementApplicationService>();
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl, bool? unauthorized)
        {
            AppUser.Clear();
            ViewBag.ReturnUrl = "/";
            if (Uri.IsWellFormedUriString(returnUrl, UriKind.RelativeOrAbsolute))
            {
                ViewBag.ReturnUrl = returnUrl;
            }

            ViewBag.Unauthorized = unauthorized ?? false;

            return View();
        }

        [AllowAnonymous]
        public JsonResult LoginUser(string userName, string password)
        {
            UserDpo userAuthenticatedDpo;
            try
            {
                var userAuthenticated = UserAccessApplicationSvc.LoginUser(CodeConst.Application.Stock.Code, AuthHelper.GenerateAuthAttribute(userName, password));
                userAuthenticatedDpo = userAuthenticated.GetDpo();

                //if ((userAuthenticated != null) && (userName == password))
                //{
                //    userAuthenticatedDpo.FirstLogin = true;
                //    
                //    if (userAuthenticatedDpo.TypeUserCustomer)
                //    {
                //        SiteLogg.Info(CodeConst.OperationCode.FirstLogInUser.Code, new Dictionary<string, string> { { "UserAuthenticatedId", userAuthenticated.Id.ToString() } });
                //        return this.Json(userAuthenticatedDpo);
                //    }

                //    EmailNotifier.NewAccountCreated(userAuthenticated, Settings.BaseUrlForSite);
                //}

                if ((userAuthenticated != null) && (!userAuthenticatedDpo.FirstLogin))
                {
                    AppUser.Set(userAuthenticated);
                    if (Session != null)
                    {
                        Session["MenuHtml"] = GenerateMenuHtml(MenuDefinition.Options, true, userAuthenticated.Roles);
                    }
                }
            }
            catch (BusinessRulesException ex)
            {
                var excode = ex.Report.FirstOrDefault();
                var exp = string.Empty;
                if (excode == null)
                {
                    exp = "No es posible iniciar sesión. Intentelo de nuevo mas tarde.";
                }
                else
                {
                    switch (excode.Code)
                    {
                        case "InvalidPassword":
                        case "InvalidUser":
                            exp = "Nombre de usuario o contraseña incorrectos.";
                            break;
                        case "InvalidUserName":
                            exp = "El usuario especificado, no pertenece a esta aplicación.";
                            break;
                    }
                }

                throw new AppException(exp);
            }

            return Json(userAuthenticatedDpo);
        }

        public ActionResult Logoff()
        {
            SiteLogg.Verbose(
              CodeConst.OperationCode.LogOutUser.Code,
              AppUser.IsLoggedIn() ? new Dictionary<string, string> { { "UserName", AppUser.Get().UserName } } : new Dictionary<string, string> { { "UserName", "NO LOGGED" } });
            AppUser.Clear();
            return Redirect("/");
        }

        private static string GenerateMenuHtml(MenuDefinition.Option[] options, bool setId, ICollection<string> userRoles)
        {
            if ((options == null) || !options.Any())
            {
                return string.Empty;
            }

            var htmlMenu = setId ? "<ul id='menu' class='nav navbar-nav'> " : "<ul>";
            foreach (var option in options)
            {
                if (option.Roles == null || (option.Roles != null && option.Roles.Any(userRoles.Contains)))
                {
                    if (option.SubOptions != null && option.SubOptions.Count() != 0)
                    {
                        htmlMenu += "<li class='dropdown'>";                        
                    }
                    else
                    {
                        htmlMenu += "<li>";
                    }
                    
                    if (string.IsNullOrWhiteSpace(option.Controller))
                    {
                        htmlMenu += string.Format("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>{0}<span class='caret'></span></a><ul class='dropdown-menu'>", option.Text);
                    }
                    else
                    {
                        htmlMenu += string.Format("<a href='/{0}/{1}'>{2}</a>", option.Controller, option.Action, option.Text);
                    }

                    htmlMenu += GenerateMenuHtml(option.SubOptions, false, userRoles);

                    if (option.SubOptions != null && option.SubOptions.Count() != 0)
                    {
                        htmlMenu = htmlMenu + "</ul></li>";
                    }
                    else
                    {
                        htmlMenu = htmlMenu + "</li>";
                    }                    
                }
            }

            htmlMenu += "</ul>";

            return htmlMenu;
        }
    }
}