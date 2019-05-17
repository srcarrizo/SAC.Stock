namespace SAC.Stock.Domain.BranchOfficeContext
{
    using LocationContext;
    using PhoneContext;
    using StaffContext;
    using Service.BranchOfficeContext;
    using System;    
    using System.Linq;
    internal static class BranchOfficeAdapter
    {
        public static BranchOfficeDto AdaptToBranchOfficeDto(this BranchOffice entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BranchOfficeDto
            {
                Id = entity.Id,
                ActivatedDate = entity.ActivatedDate,
                Address = entity.Address.AdaptToAddressDto(),                
                CreatedDate = entity.CreatedDate,
                DeactivateNote = entity.DeactivateNote,
                DeactivatedDate = entity.DeactivatedDate,
                Description = entity.Description,                
                Name = entity.Name,
                Phones = entity.Phones.Select(r => r.AdaptToPhoneDto()).ToList()                
            };
        }

        public static BranchOffice AdapterToBranchOffice(this BranchOfficeDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BranchOffice
            {
                Id = dto.Id,
                ActivatedDate = dto.ActivatedDate,                
                CreatedDate = dto.CreatedDate,
                DeactivateNote = dto.DeactivateNote,
                DeactivatedDate = dto.DeactivatedDate,
                Description = dto.Description,                
                Name = dto.Name,
                Address = dto.Address.AdaptToAddress(),
                Phones = dto.Phones.Select(r => r.AdaptTo<BranchOfficePhone>()).ToList()                
            };
        }

        public static BranchOfficeStaffDto AdaptToFranchiseStaffDto(this BranchOfficeStaff entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BranchOfficeStaffDto
            {
                Id = entity.Id,
                CreatedDate = entity.CreatedDate,
                DeactivateNote = entity.DeactivateNote,
                DeactivatedDate = entity.DeactivatedDate,                
                UserId = entity.UserId ?? Guid.Empty,
                Staff = entity.Staff.AdaptToStaffDto(),
                StaffRoleCode = entity.StaffRoleCode,
                BranchOffice = entity.BranchOffice.AdaptToBranchOfficeDto()                
            };
        }

        public static void AdaptToBranchOfficeStaff(this BranchOfficeStaffDto dto, BranchOfficeStaff to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.StaffRoleCode = dto.StaffRoleCode;
            to.DeactivatedDate = dto.DeactivatedDate;
            to.DeactivateNote = dto.DeactivateNote;            
        }

        public static BranchOfficeStaff AdaptToBranchOfficeStaff(this BranchOfficeStaffDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BranchOfficeStaff
            {
                Id = dto.Id,
                BranchOffice = dto.BranchOffice.AdapterToBranchOffice(),
                BranchOfficeId = dto.BranchOffice.Id,
                CreatedDate = dto.CreatedDate,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivateNote = dto.DeactivateNote,
                StaffId = dto.Staff.Id,
                Staff =  dto.Staff.AdaptToStaff(),
                StaffRoleCode = dto.StaffRoleCode,
                UserId = dto.UserId
            };
        }
    }
}
