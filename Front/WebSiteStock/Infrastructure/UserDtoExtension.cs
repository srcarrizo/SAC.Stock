namespace SAC.Stock.Front.Infrastructure
{
    using Membership.Service.BaseDto;
    using Code;    
    using Infrastucture;
    using System.Linq;
    using WebSiteStock.Models.Shared;

    internal static class UserDtoExtension
    {
        public static UserDpo GetDpo(this UserDto userDto)
        {
            return new UserDpo
            {
                UserName = userDto.GetUserName(),
                UserProfile = userDto.GetAttributeValue(CodeConst.Attribute.StaffRole.Code),
                GenderCode = userDto.GetAttributeValue(CodeConst.Attribute.Gender.Code),
                TypeUserStaff = userDto.GetAttributeValue(CodeConst.Attribute.StaffType.Code) != null,
                TypeUserCustomer =
                         userDto.GetAttributeValue(CodeConst.Attribute.Scope.Code) == CodeConst.Scope.Customer.Code,
                Email = userDto.Email,
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Roles = userDto.Roles.ToList(),
                UidSerie = userDto.UidSerie,
                UidCode = userDto.UidCode
            };
        }
    }
}