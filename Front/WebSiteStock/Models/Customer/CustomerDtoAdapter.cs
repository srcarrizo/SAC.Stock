namespace SAC.Stock.Front.Models.Customer
{
    using System.Linq;
    using Service.BaseDto;
    using Service.CustomerContext;
    internal static class CustomerDtoAdapter
    {
        public static CustomerDpo AdaptToCustomerDpo(this CustomerDto customer)
        {
            if (customer == null)
            {
                return null;
            }

            return new CustomerDpo
            {
                Id = customer.Id,
                Name = customer.Name,
                CreatedDate = customer.CreatedDate,
                DeativatedDate = customer.DeactivateDate,
                DeativateNote = customer.DeativateNote,
                BirthDate = customer.Person.BirthDate,
                AddressId = customer.Person.Address?.Id,
                Email = customer.Person.Email,
                FirstName = customer.Person.FirstName,
                LastName = customer.Person.LastName,
                GenderCode = customer.Person.GenderCode,
                UidCodeSerie = customer.Person.UidCode + ": " + customer.Person.UidSerie,                
                PersonId = customer.Person.Id
            };
        }

        public static CustomerDto AdaptToCustomerDto(this CustomerDpo customer)
        {
            if (customer == null)
            {
                return null;
            }

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.LastName + ", " + customer.FirstName,
                CreatedDate = customer.CreatedDate,                
                DeactivateDate = customer.DeativatedDate,
                DeativateNote = customer.DeativateNote,                
                Person = new PersonDto
                {
                    BirthDate = customer.BirthDate,
                    Address = customer.Address != null ? new AddressDto
                    {
                        Id = customer.Address.Id,
                        Apartment = customer.Address.Apartment,
                        Floor = customer.Address.Floor,
                        LocationId = customer.Address.LocationId,
                        Neighborhood = customer.Address.Neighborhood,
                        Street = customer.Address.Street,
                        StreetNumber = customer.Address.StreetNumber,
                        ZipCode = customer.Address.ZipCode
                    } : null,
                    Id = customer.PersonId,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    GenderCode = customer.GenderCode,
                    LastName = customer.LastName,
                    UidCode = customer.UidCode,
                    UidSerie = customer.UidSerie,
                    Phones = customer.Phones?.Select(c => new PhoneDto
                    {
                        AreaCode = c.AreaCode,
                        CountryCode = c.CountryCode,
                        Id = c.Id,
                        Mobile = c.Mobile,
                        Name = c.Name,
                        Number = c.Number,
                        TelcoId = c.TelcoId
                    }).ToList()
                }
            };
        }
    }
}