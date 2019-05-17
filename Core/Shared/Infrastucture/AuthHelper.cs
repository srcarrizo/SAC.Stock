namespace SAC.Stock.Infrastucture
{
    using SAC.Membership.Code;
    using SAC.Membership.Service.BaseDto;    
    using System.Collections.Generic;    
    public static class AuthHelper
    {
        public static AuthAttributeDto[] GenerateAuthAttribute(string userName, string password = null)
        {
            var result = new List<AuthAttributeDto>
                     {
                       new AuthAttributeDto
                         {
                           AuthMethodCode = CodeConst.AuthMethod.LoginPassword.Code,
                           Value = userName,
                           AttributeCode = CodeConst.AuthAttribute.UserName.Code
                         }
                     };
            if (password != null)
            {
                result.Add(
                  new AuthAttributeDto
                  {
                      AuthMethodCode = CodeConst.AuthMethod.LoginPassword.Code,
                      Value = password,
                      AttributeCode = CodeConst.AuthAttribute.Password.Code
                  });
            }

            return result.ToArray();
        }
    }
}
