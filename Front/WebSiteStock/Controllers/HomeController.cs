namespace SAC.Stock.Front.WebSiteStock.Controllers
{
    using SAC.Stock.Front.Infrastructure;    
    using System.Web.Mvc;

    [Authorize]
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            if (!AppUser.IsLoggedIn())
            {
                return this.RedirectToAction("Login", "Account");
            }

            ViewBag.Title = "Página de Inicio";

            return View();
        }
    }
}
