namespace SAC.Stock.Front.Models.Bill
{
    using SAC.Stock.Service.BillContext;
    internal static class BillDtoAdapter
    {
        public static BillDpo AdaptToBillDpo(this BillDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new BillDpo
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                Value = dto.Value,
                BillUnitType = new BillUnitTypeDpo
                {
                    Id = dto.BillUnitType.Id,
                    Code = dto.BillUnitType.Code,
                    IsDecimal = dto.BillUnitType.IsDecimal,
                    Name = dto.BillUnitType.Name
                }
            };
        }       
    }
}