namespace SAC.Stock.Front.Infrastructure.Authorize
{
    using SAC.Stock.Front.WebSiteStock.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    internal static class AuthorizeOptionHelper
    {
        public static void ApplyRoles(string optionCodes, MenuDefinition.Option[] options, ref ICollection<string> roles)
        {
            if (roles == null)
            {
                roles = new List<string>();
            }

            if ((options == null) || string.IsNullOrWhiteSpace(optionCodes))
            {
                return;
            }

            var codes = optionCodes.Split(',').Select(r => r.Trim());
            foreach (var optionCode in codes)
            {
                var op = SearchOp(optionCode, options);
                if (op != null)
                {
                    roles = roles.Union(op.Roles).ToList();
                }
            }
        }

        public static bool InRole(ICollection<string> optionRoles, string roles)
        {
            return GetRoles(optionRoles, roles).Any(s => GetUserRoles().Any(s1 => s1.Equals(s, StringComparison.InvariantCultureIgnoreCase)));
        }

        private static IEnumerable<string> GetRoles(ICollection<string> optionRoles, string roles)
        {
            return string.IsNullOrWhiteSpace(roles) ? optionRoles : optionRoles.Union(roles.Split(',').Select(r => r.Trim())).ToList();
        }

        private static MenuDefinition.Option SearchOp(string optionCode, MenuDefinition.Option[] options)
        {
            if (options == null)
            {
                return null;
            }

            var option = options.FirstOrDefault(r => r.OptionCode.Equals(optionCode, StringComparison.InvariantCultureIgnoreCase));
            if (option == null)
            {
                foreach (var op in options)
                {
                    option = SearchOp(optionCode, op.SubOptions);
                    if (option != null)
                    {
                        break;
                    }
                }
            }

            return option;
        }

        private static IEnumerable<string> GetUserRoles()
        {
            var userRoles = new List<string>();

            if (AppUser.IsLoggedIn())
            {
                userRoles.AddRange(AppUser.Get().Roles);
            }

            return userRoles;
        }
    }
}