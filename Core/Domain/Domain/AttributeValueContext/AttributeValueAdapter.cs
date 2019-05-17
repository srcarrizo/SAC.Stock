namespace SAC.Stock.Domain.AttributeValueContext
{
    using SAC.Stock.Service.BaseDto;
    internal static class AttributeValueAdapter
    {
        public static AttributeValueDto AdaptToAttributeValueDto(this AttributeValue entity)
        {
            return entity == null ? null : new AttributeValueDto { Id = entity.Id, AttributeCode = entity.AttributeCode, Value = entity.Value };
        }

        public static T AdaptTo<T>(this AttributeValueDto dto) where T : AttributeValue, new()
        {
            if (dto == null)
            {
                return null;
            }

            return new T
            {
                Id = dto.Id,
                AttributeCode = dto.AttributeCode,
                Value = dto.Value
            };
        }

        public static void AdaptToAttributeValue(this AttributeValueDto dto, AttributeValue to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.Value = dto.Value;
            to.AttributeCode = dto.AttributeCode;
            to.Id = dto.Id;
        }
    }
}