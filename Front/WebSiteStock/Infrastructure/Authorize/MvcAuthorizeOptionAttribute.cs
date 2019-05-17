namespace SAC.Stock.Front.Infrastructure.Authorize
{
    using SAC.Stock.Front.WebSiteStock.Models;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal class MvcAuthorizeOptionAttribute : AuthorizeAttribute
    {
        private readonly ICollection<string> optionRoles;

        public MvcAuthorizeOptionAttribute(string optionCodes)
        {
            AuthorizeOptionHelper.ApplyRoles(optionCodes, MenuDefinition.Options, ref this.optionRoles);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {            
            return AuthorizeOptionHelper.InRole(this.optionRoles, this.Roles);
        }
    }
}