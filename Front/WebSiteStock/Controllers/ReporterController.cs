namespace SAC.Stock.Front.Controllers
{
    using SAC.Stock.Front.Infrastructure.Authorize;
    using SAC.Stock.Front.WebSiteStock.Models;
    using System.Web.Mvc;

    [Authorize]
    public class ReporterController : Controller
    {
        [MvcAuthorizeOption(MenuDefinition.Code.Reports)]
        public ActionResult Index()
        {
            return View();
        }
    }
}