namespace SAC.Stock.Domain.PhoneContext
{
    using Service.BaseDto;
    internal static class PhoneAdapter
    {
        public static PhoneDto AdaptToPhoneDto(this Phone entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new PhoneDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AreaCode = entity.AreaCode,
                CountryCode = entity.CountryCode,
                Mobile = entity.Mobile,
                Number = entity.Number,
                TelcoId = entity.TelcoId
            };
        }

        public static TelcoDto AdaptToTelcoDto(this Telco entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TelcoDto { Id = entity.Id, Name = entity.Name, Code = entity.Code, Description = entity.Description };
        }

        public static T AdaptTo<T>(this PhoneDto dto) where T : Phone, new()
        {
            if (dto == null)
            {
                return null;
            }

            return new T
            {
                Id = dto.Id,
                Name = dto.Name,
                AreaCode = dto.AreaCode,
                CountryCode = dto.CountryCode,
                Mobile = dto.Mobile,
                Number = dto.Number,
                TelcoId = dto.TelcoId
            };
        }

        public static void AdaptToPhone(this PhoneDto dto, Phone to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.Id = dto.Id;
            to.Name = dto.Name;
            to.AreaCode = dto.AreaCode;
            to.CountryCode = dto.CountryCode;
            to.Mobile = dto.Mobile;
            to.Number = dto.Number;
            to.TelcoId = dto.TelcoId;
        }
    }
}
