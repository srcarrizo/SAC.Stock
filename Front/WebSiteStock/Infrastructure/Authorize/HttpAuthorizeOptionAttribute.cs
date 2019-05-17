namespace SAC.Stock.Front.Infrastructure.Authorize
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http;
    using System.Net;
    using System.Net.Http;
    using SAC.Stock.Front.WebSiteStock.Models;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal class HttpAuthorizeOptionAttribute : AuthorizeAttribute
    {
        private readonly ICollection<string> optionRoles;

        public HttpAuthorizeOptionAttribute(string optionCodes)
        {
            AuthorizeOptionHelper.ApplyRoles(optionCodes, MenuDefinition.Options, ref this.optionRoles);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var success = AuthorizeOptionHelper.InRole(this.optionRoles, this.Roles);

            if (success)
            {
                return;
            }

            this.HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Resources.Strings.AuthorizationHasBeenDenied);
        }
    }
}