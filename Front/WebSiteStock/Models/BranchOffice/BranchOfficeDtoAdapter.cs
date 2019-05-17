namespace SAC.Stock.Front.Models.BranchOffice
{
    using SAC.Stock.Front.Models.Shared;
    using SAC.Stock.Service.BaseDto;
    using SAC.Stock.Service.BranchOfficeContext;
    using System;
    using System.Linq;

    internal static class BranchOfficeDtoAdapter
    {
        public static BranchOfficeDpo AdaptToBranchOfficeDpo(this BranchOfficeDto branchOffice)
        {
            if (branchOffice == null)
            {
                return null;
            }

            return new BranchOfficeDpo
            {
                Id = branchOffice.Id,
                Name = branchOffice.Name,
                ActivatedDate = branchOffice.ActivatedDate,
                CreatedDate = branchOffice.CreatedDate,
                DeactivatedDate = branchOffice.DeactivatedDate,
                Description = branchOffice.Description,
                DeactivateNote = branchOffice.DeactivateNote,
                AddressId = branchOffice.Address?.Id
            };
        }

        public static BranchOfficeDto AdaptToBranchOfficeDto(this BranchOfficeDpo branchOffice)
        {
            if (branchOffice == null)
            {
                return null;
            }

            return new BranchOfficeDto
            {
                Id = branchOffice.Id,
                Name = branchOffice.Name,
                ActivatedDate = branchOffice.ActivatedDate,
                CreatedDate = branchOffice.CreatedDate,
                DeactivatedDate = branchOffice.DeactivatedDate,
                Description = branchOffice.Description,
                DeactivateNote = branchOffice.DeactivateNote,
                Address = branchOffice.Address != null ? new AddressDto
                {
                    Id = branchOffice.Address.Id,
                    Apartment = branchOffice.Address.Apartment,
                    Floor = branchOffice.Address.Floor,
                    LocationId = branchOffice.Address.LocationId,
                    Neighborhood = branchOffice.Address.Neighborhood,
                    Street = branchOffice.Address.Street,
                    StreetNumber = branchOffice.Address.StreetNumber,
                    ZipCode = branchOffice.Address.ZipCode
                } : null,
                Phones = branchOffice.Phones?.Select(c => new PhoneDto
                {
                    AreaCode = c.AreaCode,
                    CountryCode = c.CountryCode,
                    Id = c.Id,
                    Mobile = c.Mobile,
                    Name = c.Name,
                    Number = c.Number,
                    TelcoId = c.TelcoId
                }).ToList(),

            };
        }

        public static BranchOfficeStaffDpo AdaptToBranchOfficeStaffDpo(this BranchOfficeStaffDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BranchOfficeStaffDpo
            {
                Id = dto.Id,
                BranchOfficeId = dto.BranchOffice.Id,
                CreatedDate = dto.CreatedDate,
                DeactivatedDate = dto.DeactivatedDate,
                DeactivateNote = dto.DeactivateNote,
                StaffId = dto.Staff.Id,
                StaffRoleCode = dto.StaffRoleCode,
                UserId = dto.UserId,                
                Staff = new StaffDpo
                {
                    Person = new PersonDpo
                    {
                        Email = dto.Staff.Person.Email,
                        FirstName = dto.Staff.Person.FirstName,
                        Id = dto.Staff.Person.Id,
                        LastName = dto.Staff.Person.LastName,
                        UidCode =  dto.Staff.Person.UidCode,
                        UidSerie = dto.Staff.Person.UidSerie                        
                    }
                }
            };
        }

        public static BranchOfficeStaffSaveDto AdaptToBranchOfficeStaffSaveDto(this BranchOfficeStaffDpo dpo)
        {
            if (dpo == null)
            {
                return null;
            }

            var branchoffice = new BranchOfficeStaffSaveDto
            {
                Id = dpo.Id,
                BranchOffice = dpo.BranchOffice.AdaptToBranchOfficeDto(),                               
                CreatedDate = dpo.CreatedDate,
                DeactivatedDate = dpo.DeactivatedDate,
                DeactivateNote = dpo.DeactivateNote,                
                StaffRoleCode = dpo.StaffRoleCode,                
                Staff = new StaffDto
                {
                    Person = new PersonDto
                    {
                        Email = dpo.Staff.Person.Email,
                        FirstName = dpo.Staff.Person.FirstName,
                        Id = dpo.Staff.Person.Id,
                        LastName = dpo.Staff.Person.LastName,
                        UidCode = dpo.Staff.Person.UidCode,
                        UidSerie = dpo.Staff.Person.UidSerie,
                        BirthDate = dpo.Staff.Person.BirthDate,
                        Address = dpo.Staff.Person.Address != null ? new AddressDto
                        {
                            Id = dpo.Staff.Person.Address.Id,
                            Apartment = dpo.Staff.Person.Address.Apartment,
                            Floor = dpo.Staff.Person.Address.Floor,                            
                            LocationId = dpo.Staff.Person.Address.LocationId,
                            Neighborhood = dpo.Staff.Person.Address.Neighborhood,
                            Street = dpo.Staff.Person.Address.Street,
                            StreetNumber = dpo.Staff.Person.Address.StreetNumber,
                            ZipCode = dpo.Staff.Person.Address.ZipCode
                        } : null,
                        Phones = dpo.Staff.Person.Phones?.Select(c => new PhoneDto
                        {
                            AreaCode = c.AreaCode,
                            CountryCode = c.CountryCode,
                            Id = c.Id,
                            Mobile = c.Mobile,
                            Name = c.Name,
                            Number = c.Number,
                            TelcoId = c.TelcoId
                        }).ToList(),
                    }
                },
                Roles = dpo.Roles                
            };
            
            if (dpo.UserId != null)
            {
                branchoffice.UserId = (Guid)dpo.UserId;
            }

            return branchoffice;
        }
    }
}