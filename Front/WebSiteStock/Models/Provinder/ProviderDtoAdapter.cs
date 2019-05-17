namespace SAC.Stock.Front.Models.Provinder
{
    using System.Linq;
    using Service.BaseDto;
    using Service.ProviderContext;
    internal static class ProviderDtoAdapter
    {
        public static ProviderDpo AdaptToProviderDpo(this ProviderDto provider)
        {
            if (provider == null)
            {
                return null;
            }

            return new ProviderDpo
            {
                Id = provider.Id,
                Name = provider.Name,
                CreatedDate = provider.CreatedDate,
                DeativatedDate = provider.DeactivateDate,
                DeativateNote = provider.DeativateNote,
                BirthDate = provider.Person.BirthDate,
                AddressId = provider.Person.Address?.Id,
                Email = provider.Person.Email,
                FirstName = provider.Person.FirstName,
                LastName = provider.Person.LastName,
                GenderCode = provider.Person.GenderCode,
                UidCodeSerie = provider.Person.UidCode + ": " + provider.Person.UidSerie,                
                PersonId = provider.Person.Id
            };
        }

        public static ProviderDto AdaptToProviderDto(this ProviderDpo provider)
        {
            if (provider == null)
            {
                return null;
            }

            return new ProviderDto
            {
                Id = provider.Id,
                Name = provider.LastName + ", " + provider.FirstName,
                CreatedDate = provider.CreatedDate,
                Person = new PersonDto
                {
                    FirstName = provider.FirstName,
                    Address = provider.Address != null ? new AddressDto
                    {
                        Id = provider.Address.Id,
                        StreetNumber = provider.Address.StreetNumber,
                        Street = provider.Address.Street,
                        Neighborhood = provider.Address.Neighborhood,
                        LocationId = provider.Address.LocationId,
                        Apartment = provider.Address.Apartment,
                        Floor = provider.Address.Floor,
                        ZipCode = provider.Address.ZipCode
                    } : null,
                    BirthDate = provider.BirthDate,
                    Email = provider.Email,
                    GenderCode = provider.GenderCode,
                    LastName = provider.LastName,
                    UidCode = provider.UidCode,
                    UidSerie = provider.UidSerie,
                    Phones = provider.Phones?.Select(c => new PhoneDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        AreaCode = c.AreaCode,
                        CountryCode = c.CountryCode,
                        Mobile = c.Mobile,
                        Number = c.Number,
                        TelcoId = c.TelcoId                        
                    }).ToList()
                },
                DeactivateDate = provider.DeativatedDate,
                DeativateNote = provider.DeativateNote
            };
        }
    }
}