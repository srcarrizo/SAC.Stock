using System.Web;
using System.Web.Mvc;

namespace SAC.Stock.Front.WebSiteStock
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
