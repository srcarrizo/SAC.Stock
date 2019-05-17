namespace SAC.Stock.Front.WebSiteStock.Models
{
    using System;    
    using System.Configuration;    
    using System.Web;
    internal static class Settings
    {
        public static string PublicPath
        {
            get
            {
                return GetValue("PublicPath", "~/Public");
            }
        }

        public static string BaseUrlForSite
        {
            get
            {
                return GetValue("BaseUrlForSite", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
            }
        }

        public static string IsSecureConnection
        {
            get
            {
                return GetValue("IsSecureConnection", false.ToString());
            }
        }
      
        private static string GetValue(string key, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[key]) ? defaultValue : ConfigurationManager.AppSettings[key];
        }
    }
}