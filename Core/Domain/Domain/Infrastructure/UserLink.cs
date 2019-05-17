namespace SAC.Stock.Domain.Infrastructure
{
    using SAC.Membership.Helper;
    using SAC.Membership.Service.BaseDto;
    using SAC.Membership.Service.UserManagement;
    using SAC.Stock.Code;
    using SAC.Stock.Infrastucture;
    using System;
    internal class UserLink
    {
        private readonly IUserManagementApplicationService userManagementSvc;
        private UserDto userFromSvc;
        public UserLink(IUserManagementApplicationService userManagementSvcParam, string formatString, params object[] args)
        {
            this.userManagementSvc = userManagementSvcParam;
            this.UserName = Code.UserName.Get(formatString, args);
        }

        public string UserName { get; private set; }

        public bool EvalIfActivated()
        {
            this.userFromSvc = this.userFromSvc ?? this.userManagementSvc.GetUser(this.UserName);
            if (this.userFromSvc == null)
            {
                return false;
            }

            var cryptoPass = this.userFromSvc.GetCryptoPass();
            return this.userFromSvc.IsActive
                   && !cryptoPass.Equals(
                     UserHelper.GetCryptoPass(this.UserName, this.UserName, cryptoPass),
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public UserDto Save(dynamic data)
        {
            var userInfo = this.userFromSvc ?? new UserDto();

            userInfo.FirstName = data.FirstName;
            userInfo.LastName = data.LastName;
            userInfo.Email = data.Email;
            userInfo.UidCode = data.UidCode;
            userInfo.UidSerie = data.UidSerie;
            userInfo.Roles = data.Roles;
            userInfo.AppCode = CodeConst.Application.Stock.Code;

            if (data.AddAttributes != null)
            {
                foreach (AttributeValueDto att in data.AddAttributes)
                {
                    userInfo.SetAttributeValue(att.AttributeCode, att.Value);
                }
            }

            if (data.GetType().GetProperty("ScopeCode") != null)
            {
                userInfo.SetAttributeValue(CodeConst.Attribute.Scope.Code, (string)data.ScopeCode);
            }

            if (data.GetType().GetProperty("StaffRoleCode") != null)
            {
                userInfo.SetAttributeValue(CodeConst.Attribute.StaffRole.Code, (string)data.StaffRoleCode);
            }

            var password = data.GetType().GetProperty("Password") == null ? this.UserName : (string)data.Password;

            userInfo.SetUserName(this.UserName);
            userInfo.SetPassword(password);

            return this.userFromSvc == null ? this.userManagementSvc.AddUser(userInfo) : this.userManagementSvc.ModifyUser(userInfo);
        }
    }
}
