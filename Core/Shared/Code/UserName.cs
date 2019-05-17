namespace SAC.Stock.Code
{
    using System;
    public static class UserName
    {
        public const string StockCrew = "STKCREW";
        public const string StockManagement = "STKADMIN";        
        public static string Get(string mask, params object[] args)
        {
            return RemoveNonAlpha(string.Format(mask, args).ToUpper());
        }
        private static string RemoveNonAlpha(string str)
        {
            var r = str.ToCharArray();
            r = Array.FindAll(r, char.IsLetterOrDigit);
            return new string(r);
        }
        public static class Mask
        {
            public const string StockAdmin = "SA{0}";
            public const string StockCrew = "SC{0}{1}";
        }
    }
}
