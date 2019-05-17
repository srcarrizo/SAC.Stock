namespace SAC.Stock.Domain.LocationContext
{
    using Service.BaseDto;
    internal static class LocationAdapter
    {
        public static Location AdaptToLocation(this LocationDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Location
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Code = dto.Code,               
                ParentLocationId = dto.ParentLocationId,                
                LocationTypeCode = dto.LocationTypeCode,
                ParentLocation = dto.ParentLocation.AdaptToLocation()
            };
        }

        public static LocationDto AdaptToLocationDto(this Location entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new LocationDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Code = entity.Code,                
                ParentLocationId = entity.ParentLocationId,                
                LocationTypeCode = entity.LocationTypeCode,
                ParentLocation = entity.ParentLocation.AdaptToLocationDto()
            };
        }       
        
        public static AddressDto AdaptToAddressDto(this Address entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AddressDto
            {
                Id = entity.Id,
                Apartment = entity.Apartment,
                Floor = entity.Floor,
                LocationId = entity.LocationId,
                Neighborhood = entity.Neighborhood,
                Street = entity.Street,
                StreetNumber = entity.StreetNumber,
                ZipCode = entity.ZipCode,
                Location = entity.Location.AdaptToLocationDto()                
            };
        }

        public static Address AdaptToAddress(this AddressDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Address
            {
                Id = dto.Id,
                Apartment = dto.Apartment,
                Floor = dto.Floor,
                LocationId = dto.LocationId,
                Neighborhood = dto.Neighborhood,
                Street = dto.Street,
                StreetNumber = dto.StreetNumber,
                ZipCode = dto.ZipCode,
                Location = dto.Location.AdaptToLocation()
            };
        }
    }
}
