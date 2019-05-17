namespace SAC.Stock.Front.WebSiteStock.Models.Shared
{
    using Membership.Helper;
    using System;
    using System.Collections.Generic;
    public class UserDpo
    {
        public UserDpo()
        {
            Roles = new List<string>();
        }
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string OriginalUserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UidCode { get; set; }

        public string UidSerie { get; set; }

        public string GenderCode { get; set; }

        public string Email { get; set; }

        public string UserProfile { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }

        public bool FirstLogin { get; set; }

        public bool TypeUserStaff { get; set; }

        public bool TypeUserCustomer { get; set; }

        public string EmailAddressValidated { get; set; }

        public bool ToValidateEmailAddress { get; set; }

        public string NewEmailAddress { get; set; }

        public string Uid
        {
            get
            {
                return UidHelper.Encode(UidCode, UidSerie);
            }
        }
    }
}