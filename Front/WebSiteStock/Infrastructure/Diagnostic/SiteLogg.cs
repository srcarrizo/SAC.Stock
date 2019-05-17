using SAC.Seed.CodeTable;
using SAC.Seed.Logging;
using SAC.Stock.Infrastucture;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Stock.Front.Infrastructure.Diagnostic
{
    internal static class SiteLogg
    {
        public static LogActivity BeginActivity(CodeItem operation, IDictionary<string, string> args = null)
        {
            return Logg.BeginActivity(operation, args);
        }

        public static void EndActivity(LogActivity activity, IDictionary<string, string> data = null)
        {
            Logg.EndActivity(activity, data);
        }

        public static LogProcess BeginProcess(CodeItem operation, IDictionary<string, string> args = null)
        {
            return Logg.BeginProcess(operation, args);
        }

        public static void EndProcess(LogProcess process, IDictionary<string, string> data = null)
        {
            Logg.EndProcess(process, data);
        }

        public static void Info(string message, IDictionary<string, string> data = null)
        {
            Logg.Info(message, GetData(data));
        }

        public static void Verbose(string message, IDictionary<string, string> data = null)
        {
            Logg.Verbose(message, GetData(data));
        }

        public static void Warning(string message, IDictionary<string, string> data = null)
        {
            Logg.Warning(message, GetData(data));
        }

        public static void Error(System.Exception ex, IDictionary<string, string> data = null)
        {
            Logg.Error(ex, GetData(data));
        }

        public static void Error(System.Exception ex, CodeItem operation, IDictionary<string, string> data = null)
        {
            Logg.Error(ex, operation, GetData(data));
        }

        private static IDictionary<string, string> GetData(IDictionary<string, string> data)
        {
            HttpRequest req = null;
            try
            {
                req = HttpContext.Current.Request;
            }
            catch
            {
                // Nothing.
            }

            var result = new Dictionary<string, string>
                     {
                       { "UserName", AppUser.IsLoggedIn() ? AppUser.Get().UserName : "No logged in" },
                       { "UserId", AppUser.IsLoggedIn() ? AppUser.Get().Id.ToString() : "No logged in" },
                       { "ClientIP", req == null ? string.Empty : req.UserHostAddress },
                       { "UrlRequest", req == null ? string.Empty : req.Url.PathAndQuery }
                     };

            if (data == null)
            {
                return result;
            }

            foreach (var dt in data.Where(dt => !result.Keys.Any(k => k.Equals(dt.Key))))
            {
                result.Add(dt.Key, dt.Value);
            }

            return result;
        }
    }
}