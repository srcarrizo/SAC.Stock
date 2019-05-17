namespace SAC.Stock.Front.Infrastructure
{
    using SAC.Membership.Service.BaseDto;    
    using System.Collections.Generic;    
    using System.Web;
    using System.Web.Security;
    using SAC.Stock.Code;
    using SAC.Stock.Front.Infrastructure.Diagnostic;

    public static class AppUser
    {
        public static UserDto Get()
        {
            var user = GetUser();

            if (user == null)
            {
                throw BusinessRulesCode.NoUserLoggedIn.NewBusinessException();
            }

            return user;
        }

        public static void Set(UserDto user)
        {
            FormsAuthentication.SetAuthCookie(user.UserName, false);

            var session = HttpContext.Current.Session;
            if (session == null)
            {
                return;
            }

            session["CurrentUser"] = user;

            SiteLogg.Verbose(CodeConst.OperationCode.LogInUser.Code, new Dictionary<string, string> { { "UserName", user.UserName } });
        }

        public static void Clear()
        {
            FormsAuthentication.SignOut();
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                session.Abandon();
            }
        }

        public static bool IsLoggedIn()
        {
            return GetUser() != null;
        }

        private static UserDto GetUser()
        {
            var session = HttpContext.Current.Session;
            UserDto user = null;
            if (session != null)
            {
                user = (UserDto)session["CurrentUser"];
            }

            return user;
        }
    }
}