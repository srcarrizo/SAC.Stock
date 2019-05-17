namespace SAC.Stock.Domain.PersonContext
{
    using LocationContext;
    using PhoneContext;
    using Service.BaseDto;    
    using System.Linq;    
    internal static class PersonAdapter
    {
        public static PersonDto AdaptPersonDto(this Person person)
        {
            if (person == null)
            {
                return null;
            }

            return new PersonDto
            {
                Id = person.Id,
                Email = person.Email,
                UidCode = person.UidCode,
                UidSerie = person.UidSerie,
                LastName = person.LastName,
                GenderCode = person.GenderCode,
                FirstName = person.FirstName,
                BirthDate = person.BirthDate,
                Address = person.Address.AdaptToAddressDto(),
                Phones = person.Phones.Select(r => r.AdaptToPhoneDto()).ToList()                             
            };
        }

        public static PersonWithTypeDto AdaptPersonWithTypeDto(this Person person)
        {
            if (person == null)
            {
                return null;
            }

            return new PersonWithTypeDto
            {
                Id = person.Id,
                Email = person.Email,
                UidCode = person.UidCode,
                UidSerie = person.UidSerie,
                LastName = person.LastName,
                GenderCode = person.GenderCode,
                FirstName = person.FirstName,
                BirthDate = person.BirthDate,
                Address = person.Address.AdaptToAddressDto(),
                Phones = person.Phones.Select(r => r.AdaptToPhoneDto()).ToList(),                
                IsStaff = person.Staff != null
            };
        }

        public static Person AdaptPerson(this PersonDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Person
            {
                Id = dto.Id,
                Email = dto.Email,
                UidCode = dto.UidCode,
                UidSerie = dto.UidSerie,
                LastName = dto.LastName,
                GenderCode = dto.GenderCode,
                FirstName = dto.FirstName,
                BirthDate = dto.BirthDate,
                Address = dto.Address.AdaptToAddress(),
                Phones = dto.Phones.Select(r => r.AdaptTo<PersonPhone>()).ToList()                
            };
        }
    }
}