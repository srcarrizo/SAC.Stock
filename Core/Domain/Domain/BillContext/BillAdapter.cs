namespace SAC.Stock.Domain.BillContext
{
    using SAC.Stock.Service.BillContext;    
    internal static class BillAdapter
    {
        public static BillDto AdaptToBillDto(this Bill entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BillDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Value = entity.Value,
                BillUnitType = entity.BillUnitType.AdaptToBillUnitTypeDto()
            };
        }

        public static BillUnitTypeDto AdaptToBillUnitTypeDto(this BillUnitType entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new BillUnitTypeDto
            {
                Code = entity.Code,
                Id = entity.Id,
                IsDecimal = entity.IsDecimal,
                Name = entity.Name
            };
        }

        public static Bill AdaptToBill(this BillDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Bill
            {
                BillUnitTypeId = dto.BillUnitType.Id,                
                Code = dto.Code,
                Id = dto.Id,
                Name = dto.Name,
                Value = dto.Value                
            };
        }

        public static BillUnitType AdaptToBillUnitType(this BillUnitTypeDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BillUnitType
            {
                Code = dto.Code,
                Id = dto.Id,
                IsDecimal = dto.IsDecimal,
                Name = dto.Name
            };
        }
    }
}
