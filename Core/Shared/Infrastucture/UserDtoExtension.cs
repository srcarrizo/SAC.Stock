namespace SAC.Stock.Infrastucture
{
    using SAC.Membership.Service.BaseDto;
    using Code;
    public static class UserDtoExtension
    {
        public static void SetUserName(this UserDto userDto, string value)
        {
            userDto.UserName = value;
            userDto.SetAuthAttributeValue(CodeConst.AuthMethod.LoginPassword.Code, CodeConst.AuthAttribute.UserName.Code, value);
        }

        public static void SetPassword(this UserDto userDto, string value)
        {
            userDto.SetAuthAttributeValue(CodeConst.AuthMethod.LoginPassword.Code, CodeConst.AuthAttribute.Password.Code, value);
        }

        public static string GetUserName(this UserDto userDto)
        {
            return userDto.GetAuthAttributeValue(CodeConst.AuthMethod.LoginPassword.Code, CodeConst.AuthAttribute.UserName.Code);
        }

        public static string GetCryptoPass(this UserDto userDto)
        {
            return userDto.GetAuthAttributeValue(CodeConst.AuthMethod.LoginPassword.Code, CodeConst.AuthAttribute.CryptoPass.Code);
        }
    }
}