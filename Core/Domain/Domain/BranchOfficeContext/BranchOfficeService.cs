namespace SAC.Stock.Domain.BranchOfficeContext
{
    using Membership.Service.BaseDto;
    using Membership.Service.UserManagement;
    using Seed.NLayer.Data;
    using Seed.NLayer.Data.Ordering;
    using Seed.NLayer.Domain;
    using Seed.NLayer.Domain.Specification;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using Infrastructure;
    using LocationContext;
    using PhoneContext;
    using StaffContext;
    using Service.BaseDto;
    using Service.BranchOfficeContext;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AttributeValueDto = Membership.Service.BaseDto.AttributeValueDto;

    internal class BranchOfficeService
    {
        private readonly IDataContext context;

        internal BranchOfficeService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<BranchOfficePhone, int> BranchOfficePhone
        {
            get
            {
                return context.GetView<BranchOfficePhone, int>();
            }
        }

        private IDataView<BranchOffice, Guid> ViewBranchOffice
        {
            get
            {
                return context.GetView<BranchOffice, Guid>();
            }
        }

        private IDataView<BranchOfficeStaff, Guid> ViewBranchOfficeStaff
        {
            get
            {
                return context.GetView<BranchOfficeStaff, Guid>();
            }
        }

        internal BranchOfficeStaff AddBranchOfficeStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo, IUserManagementApplicationService userManagementSvc)
        {
            var BranchOfficeStaff = NewBranchOfficeStaff(branchOfficeStaffInfo, userManagementSvc);
            context.ApplyChanges();

            return BranchOfficeStaff;
        }

        internal BranchOffice AddBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            var branchOffice = NewBranchOffice(branchOfficeInfo);
            ViewBranchOffice.Add(branchOffice);
            context.ApplyChanges();

            return branchOffice;
        }

        internal BranchOfficeStaff ModifyBranchOfficeStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo, IUserManagementApplicationService userManagementSvc)
        {
            var BranchOfficeStaff = UpdateBranchOfficeStaff(branchOfficeStaffInfo, userManagementSvc);
            ViewBranchOfficeStaff.Modify(BranchOfficeStaff);
            context.ApplyChanges();

            return BranchOfficeStaff;
        }

        internal BranchOffice ModifyBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            var branchOffice = UpdateBranchOffice(branchOfficeInfo);
            ViewBranchOffice.Modify(branchOffice);
            context.ApplyChanges();

            return branchOffice;
        }

        internal BranchOfficeStaff GetBranchOfficeStaff(Guid id)
        {
            return ViewBranchOfficeStaff.Get(id);
        }

        internal BranchOfficeStaff GetBranchOfficeStaff(string uidCode, string uidSerie)
        {
            return ViewBranchOfficeStaff.Query(SpecExistingBranchOfficeStaff(uidCode, uidSerie)).FirstOrDefault();
        }

        internal BranchOffice GetBranchOffice(Guid id)
        {
            return ViewBranchOffice.Get(id);
        }

        internal IEnumerable<BranchOffice> QueryBranchOffice(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<BranchOffice>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.BranchOffice.Name, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BranchOffice, string>(pt => pt.Name, info.Direction));
                }               
            }

            return ViewBranchOffice.Query(
              pageIndex, pageSize, orderByExpressions.ToArray(), GetBranchOfficeSpecification(filterInfo));
        }

        internal IEnumerable<BranchOfficeStaff> QueryBranchOfficeStaff(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<BranchOfficeStaff>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.BranchOfficeStaff.LastName, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BranchOfficeStaff, string>(pt => pt.Staff.Person.LastName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.BranchOfficeStaff.FirstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BranchOfficeStaff, string>(pt => pt.Staff.Person.FirstName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.BranchOfficeStaff.FirstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BranchOfficeStaff, string>(pt => pt.Staff.Person.FirstName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.BranchOfficeStaff.Uid, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BranchOfficeStaff, string>(pt => pt.Staff.Person.UidCode, info.Direction));
                    orderByExpressions.Add(new OrderByExpression<BranchOfficeStaff, string>(pt => pt.Staff.Person.UidSerie, info.Direction));
                }

                if (info.Field.Equals(SortQuery.BranchOfficeStaff.StaffRoleCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BranchOfficeStaff, string>(pt => pt.StaffRoleCode, info.Direction));
                }
            }

            return this.ViewBranchOfficeStaff.Query(
              pageIndex, pageSize, orderByExpressions.ToArray(), GetBranchOfficeStaffSpecification(filterInfo));
        }

        internal BranchOfficeStaff NewBranchOfficeStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo, IUserManagementApplicationService userManagementSvc)
        {
            if (ViewBranchOfficeStaff.Exists(SpecBranchOfficeStaffPrevious(branchOfficeStaffInfo.Staff.Person.UidCode, branchOfficeStaffInfo.Staff.Person.UidSerie)))
            {
                throw new BusinessRulesException(BusinessRulesCode.BranchOfficeStaffPrevious.Message, BusinessRulesCode.BranchOfficeStaffPrevious.Code);
            }

            TrimBranchOfficeStaff(branchOfficeStaffInfo);
            var userLink = new UserLink(
              userManagementSvc, UserName.Mask.StockCrew, branchOfficeStaffInfo.BranchOffice.Id.ToString().Substring(0, 4), branchOfficeStaffInfo.Staff.Person.UidSerie);

            if (userLink.EvalIfActivated())
            {
                throw new BusinessRulesException(BusinessRulesCode.UserExists.Message, BusinessRulesCode.UserExists.Code);
            }

            var staffSvc = new StaffService(this.context);
            var staff = staffSvc.GetOrNewStaff(branchOfficeStaffInfo.Staff);
           
            var branchOfficeStaff = new BranchOfficeStaff
            {
                CreatedDate = DateTimeOffset.UtcNow,
                BranchOffice = GetBranchOffice(branchOfficeStaffInfo.BranchOffice.Id),
                BranchOfficeId = branchOfficeStaffInfo.BranchOffice.Id,
                Staff = staff,
                StaffId = staff.Id,
                StaffRoleCode = branchOfficeStaffInfo.StaffRoleCode,                
            };

            branchOfficeStaff.GenerateNewIdentity();

            var userInfo =
              userLink.Save(
                new
                {
                    branchOfficeStaffInfo.Staff.Person.FirstName,
                    branchOfficeStaffInfo.Staff.Person.LastName,
                    branchOfficeStaffInfo.Staff.Person.Email,
                    branchOfficeStaffInfo.Staff.Person.UidCode,
                    branchOfficeStaffInfo.Staff.Person.UidSerie,
                    branchOfficeStaffInfo.Roles,

                    ScopeCode = CodeConst.Scope.BranchOffice.Code,
                    branchOfficeStaffInfo.StaffRoleCode,
                    AddAttributes =
                      new List<AttributeValueDto>
                        {
                    new AttributeValueDto
                      {
                        ApplyToUser = true,
                        AttributeCode = CodeConst.Attribute.Scope.Code,
                        Value = CodeConst.Scope.BranchOffice.Code
                      },
                      new AttributeValueDto
                      {
                        ApplyToUser = true,
                        AttributeCode = CodeConst.Attribute.PersonId.Code,
                        Value = staff.Person.Id.ToString()
                      },
                    new AttributeValueDto
                      {
                        ApplyToUser = true,
                        AttributeCode = CodeConst.Attribute.StaffType.Code,
                        Value = CodeConst.StaffType.BranchOfficeStaff.Code
                      },
                    new AttributeValueDto
                      {
                        ApplyToUser = true,
                        AttributeCode = CodeConst.Attribute.BranchOfficeStaffId.Code,
                        Value = branchOfficeStaff.Id.ToString()
                      },                    
                    new AttributeValueDto
                      {
                        ApplyToUser = true,
                        AttributeCode = CodeConst.Attribute.StaffId.Code,
                        Value = staff.Id.ToString()
                      },
                    new AttributeValueDto
                      {
                        ApplyToUser = true,
                        AttributeCode = CodeConst.Attribute.BranchOfficeId.Code,
                        Value = branchOfficeStaffInfo.BranchOffice.Id.ToString()
                      }
                    }
                });

            branchOfficeStaff.UserId = userInfo.Id;

            ViewBranchOfficeStaff.Add(branchOfficeStaff);

            return branchOfficeStaff;
        }

        private static Specification<BranchOfficeStaff> GetBranchOfficeStaffSpecification(FilterInfo filterInfo)
        {
            Specification<BranchOfficeStaff> spec = new TrueSpecification<BranchOfficeStaff>();            
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.BranchOfficeStaff.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBranchOfficeStaffByFullSearch(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.BranchOfficeStaff.BranchOfficeId, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBranchOfficeStaffByBranchOffice(filterInfo.Value);
                }
            }
             
            return spec;
        }

        private static Specification<BranchOffice> GetBranchOfficeSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<BranchOffice> spec = new TrueSpecification<BranchOffice>();
            if (filterInfo != null)
            {
                foreach (var filter in filterInfo)
                {
                    if (filter.Spec.Equals(SpecFilter.BranchOffice.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecBranchOfficeFilterByName(filter.Value);
                    }
                }
            }

            return spec;
        }

        private static Specification<BranchOffice> SpecBranchOfficeFilterByName(string value)
        {
            return new DirectSpecification<BranchOffice>(c => c.Name.ToLower().Contains(value.ToLower()));
        }

        private static Specification<BranchOfficeStaff> SpecBranchOfficeStaffByFullSearch(string value)
        {
            Specification<BranchOfficeStaff> result = new DirectSpecification<BranchOfficeStaff>(s => s.Staff.Person.LastName.ToLower().Contains(value.ToLower()));
            result |= new DirectSpecification<BranchOfficeStaff>(s => s.Staff.Person.FirstName.ToLower().Contains(value.ToLower()));
            result |= new DirectSpecification<BranchOfficeStaff>(s => s.Staff.Person.UidSerie.ToLower().Contains(value.ToLower()));

            return result;
        }

        private static Specification<BranchOfficeStaff> SpecBranchOfficeStaffByBranchOffice(string value)
        {
            var branchOffice = Guid.Parse(value);
            return new DirectSpecification<BranchOfficeStaff>(c => c.BranchOffice.Id.Equals(branchOffice));
        }

        private static Specification<BranchOfficeStaff> SpecExistingBranchOfficeStaff(string uidCode, string uidSerie)
        {
            return
              new DirectSpecification<BranchOfficeStaff>(
                r =>
                r.Staff.Person.UidSerie.Equals(uidSerie, StringComparison.InvariantCultureIgnoreCase)
                && r.Staff.Person.UidCode.Equals(uidCode, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Specification<BranchOfficeStaff> SpecBranchOfficeStaffPrevious(string uidCode, string uidSerie)
        {
            Specification<BranchOfficeStaff> result =
              new AndSpecification<BranchOfficeStaff>(
                new DirectSpecification<BranchOfficeStaff>(
                  r =>
                  r.Staff.Person.UidSerie.Equals(uidSerie, StringComparison.InvariantCultureIgnoreCase)
                  && r.Staff.Person.UidCode.Equals(uidCode, StringComparison.InvariantCultureIgnoreCase)),
                new DirectSpecification<BranchOfficeStaff>(r => r.DeactivatedDate == null));
            return result;
        }

        private static void TrimBranchOfficeStaff(BranchOfficeStaffDto branchOfficeStaffInfo)
        {
            branchOfficeStaffInfo.DeactivateNote = string.IsNullOrWhiteSpace(branchOfficeStaffInfo.DeactivateNote) ? null : branchOfficeStaffInfo.DeactivateNote.Trim();
            branchOfficeStaffInfo.Staff.Person.FirstName = string.IsNullOrWhiteSpace(branchOfficeStaffInfo.Staff.Person.FirstName) ? null : FirstToCamelCase(branchOfficeStaffInfo.Staff.Person.FirstName.Trim());
            branchOfficeStaffInfo.Staff.Person.LastName = string.IsNullOrWhiteSpace(branchOfficeStaffInfo.Staff.Person.LastName) ? null : FirstToCamelCase(branchOfficeStaffInfo.Staff.Person.LastName.Trim());
            branchOfficeStaffInfo.Staff.Person.Email = string.IsNullOrWhiteSpace(branchOfficeStaffInfo.Staff.Person.Email) ? null : branchOfficeStaffInfo.Staff.Person.Email.ToLowerInvariant().Trim();
            branchOfficeStaffInfo.Staff.Person.UidSerie = string.IsNullOrWhiteSpace(branchOfficeStaffInfo.Staff.Person.UidSerie) ? null : branchOfficeStaffInfo.Staff.Person.UidSerie.Trim();

            if (branchOfficeStaffInfo.Staff.Person.Address != null)
            {
                TrimAddress(branchOfficeStaffInfo.Staff.Person.Address);
            }

            if (branchOfficeStaffInfo.Staff.Person.Phones == null || branchOfficeStaffInfo.Staff.Person.Phones.Count <= 0)
            {
                return;
            }

            TrimPhones(branchOfficeStaffInfo.Staff.Person.Phones);
        }

        private static void TrimBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            branchOfficeInfo.DeactivateNote = string.IsNullOrWhiteSpace(branchOfficeInfo.DeactivateNote) ? null : branchOfficeInfo.DeactivateNote.Trim();
            branchOfficeInfo.Description = string.IsNullOrWhiteSpace(branchOfficeInfo.Description) ? null : branchOfficeInfo.Description.Trim();
            branchOfficeInfo.Name = string.IsNullOrWhiteSpace(branchOfficeInfo.Name) ? null : FirstToCapital(branchOfficeInfo.Name.Trim());            

            if (branchOfficeInfo.Address != null)
            {
                TrimAddress(branchOfficeInfo.Address);
            }

            if (branchOfficeInfo.Phones.Count <= 0)
            {
                return;
            }

            TrimPhones(branchOfficeInfo.Phones);
        }

        private static void TrimPhones(IEnumerable<PhoneDto> phonesInfoList)
        {
            foreach (var phone in phonesInfoList)
            {
                phone.AreaCode = phone.AreaCode == null ? null : phone.AreaCode.Trim();
                phone.CountryCode = phone.CountryCode == null ? null : phone.CountryCode.Trim();
                phone.Name = phone.Name == null ? null : FirstToCapital(phone.Name.Trim());
                phone.Number = phone.Number == null ? null : phone.Number.Trim();
            }
        }

        private static void TrimAddress(AddressDto address)
        {
            address.Apartment = string.IsNullOrWhiteSpace(address.Apartment)
                                                   ? null
                                                   : FirstToCapital(address.Apartment.Trim());
            address.Floor = string.IsNullOrWhiteSpace(address.Floor) ? null : address.Floor.Trim();
            address.Neighborhood = string.IsNullOrWhiteSpace(address.Neighborhood)
                                                      ? null
                                                      : FirstToCapital(address.Neighborhood.Trim());
            address.Street = string.IsNullOrWhiteSpace(address.Street) ? null : FirstToCapital(address.Street.Trim());
            address.StreetNumber = string.IsNullOrWhiteSpace(address.StreetNumber) ? null : address.StreetNumber.Trim();
            address.ZipCode = string.IsNullOrWhiteSpace(address.ZipCode) ? null : address.ZipCode.Trim();            
        }

        private static string FirstToCapital(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return data;
            }

            var chars = data.ToCharArray();
            chars[0] = chars[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant().ToCharArray()[0];
            return new string(chars);
        }

        private static string FirstToCamelCase(string data)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.ToLowerInvariant());
        }

        private BranchOfficeStaff UpdateBranchOfficeStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo, IUserManagementApplicationService userManagementSvc)
        {
            var BranchOfficeStaff = GetBranchOfficeStaff(branchOfficeStaffInfo.Id);
            if (BranchOfficeStaff == null)
            {
                throw new BusinessRulesException(BusinessRulesCode.BranchOfficeStaffNotExists.Message, BusinessRulesCode.BranchOfficeStaffNotExists.Code);
            }

            var user = userManagementSvc.GetUser(branchOfficeStaffInfo.UserId);
            user.Email = branchOfficeStaffInfo.Staff.Person.Email;
            user.UidSerie = branchOfficeStaffInfo.Staff.Person.UidSerie;
            user.UidCode = branchOfficeStaffInfo.Staff.Person.UidCode;
            user.FirstName = branchOfficeStaffInfo.Staff.Person.FirstName;
            user.LastName = branchOfficeStaffInfo.Staff.Person.LastName;
            user.Roles = branchOfficeStaffInfo.Roles;
            user.SetAttributeValue(CodeConst.Attribute.StaffRole.Code, branchOfficeStaffInfo.StaffRoleCode);
            userManagementSvc.ModifyUser(user);

            var staffSvc = new StaffService(this.context);
            staffSvc.UpdateStaff(branchOfficeStaffInfo.Staff, BranchOfficeStaff.Staff);

            branchOfficeStaffInfo.DeactivatedDate = (branchOfficeStaffInfo.DeactivateNote == null) ? (DateTimeOffset?)null : DateTimeOffset.UtcNow;
            branchOfficeStaffInfo.AdaptToBranchOfficeStaff(BranchOfficeStaff);

            return BranchOfficeStaff;
        }

        private BranchOffice UpdateBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            TrimBranchOffice(branchOfficeInfo);
            var branchOffice = this.GetBranchOffice(branchOfficeInfo.Id);

            if (branchOffice == null)
            {
                throw BusinessRulesCode.BranchOfficeByIdNotExists.NewBusinessException(branchOfficeInfo.Name);
            }

            if ((branchOfficeInfo.DeactivateNote == null) && (branchOfficeInfo.DeactivateNote == branchOffice.DeactivateNote))
            {
                if ((branchOffice.ActivatedDate != null) && (branchOfficeInfo.ActivatedDate.HasValue && branchOfficeInfo.ActivatedDate.Value.Date != branchOffice.ActivatedDate.Value.Date))
                {
                    if (branchOffice.DeactivatedDate == null)
                    {
                        throw BusinessRulesCode.BranchOfficeShouldNotChangeActivate.NewBusinessException();
                    }
                }
            }

            if ((branchOffice.ActivatedDate == null) && (branchOfficeInfo.DeactivateNote != null))
            {
                BusinessRulesCode.BranchOfficeDeactivateWithoutActivate.NewBusinessException();
            }

            this.MergeBranchOfficePhone(branchOfficeInfo, branchOffice);

            if (branchOfficeInfo.DeactivateNote != null)
            {
                branchOfficeInfo.DeactivatedDate = DateTimeOffset.UtcNow;
            }
            else
            {
                branchOfficeInfo.DeactivatedDate = null;
            }

            var locationSvc = new LocationService(this.context);
            branchOffice.Address = locationSvc.UpdateOrNewAddress(branchOfficeInfo.Address, branchOffice.Address);
            branchOffice.Name = branchOfficeInfo.Name;
            branchOffice.Description = branchOfficeInfo.Description;
            branchOffice.DeactivateNote = branchOfficeInfo.DeactivateNote;
            branchOffice.DeactivatedDate = branchOfficeInfo.DeactivatedDate;
            branchOffice.ActivatedDate = branchOfficeInfo.ActivatedDate;            

            return branchOffice;
        }

        private BranchOffice NewBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            TrimBranchOffice(branchOfficeInfo);
            if (this.EvalExistsBranchOffice(branchOfficeInfo.Id))
            {
                throw BusinessRulesCode.DuplicateCode.NewBusinessException();
            }

            branchOfficeInfo.DeactivateNote = null;
            var branchOffice = branchOfficeInfo.AdapterToBranchOffice();

            branchOffice.GenerateNewIdentity();
            branchOffice.CreatedDate = DateTimeOffset.UtcNow;

            return branchOffice;
        }

        private bool EvalExistsBranchOffice(Guid branchoOfficeId)
        {
            var existBranchOffice = GetBranchOffice(branchoOfficeId);
            return (existBranchOffice != null); 
        }

        private void MergeBranchOfficePhone(BranchOfficeDto branchOfficeInfo, BranchOffice branchOffice)
        {
            var toDelete = new List<int>();
            foreach (var phone in branchOffice.Phones)
            {
                var actual = branchOfficeInfo.Phones.FirstOrDefault(r => r.Id == phone.Id);
                if (actual == null)
                {
                    toDelete.Add(phone.Id);
                }
                else
                {
                    actual.AdaptToPhone(phone);
                    BranchOfficePhone.Modify(phone);
                }
            }

            foreach (var phone in branchOfficeInfo.Phones.Where(r => r.Id == 0).Select(r => r.AdaptTo<BranchOfficePhone>()))
            {
                phone.BranchOfficeId = branchOffice.Id;
                BranchOfficePhone.Add(phone);
            }

            foreach (var id in toDelete)
            {
                BranchOfficePhone.Remove(id);
            }
        }
    }
}