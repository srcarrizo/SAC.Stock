namespace SAC.Stock.Domain.ProviderContext
{
    using PersonContext;
    using Service.ProviderContext;

    internal static class ProviderAdapter
    {
        public static ProviderDto AdaptToProviderDto(this Provider provider)
        {
            if (provider == null)
            {
                return null;
            }
            
            return new ProviderDto
            {
                Person = provider.Person.AdaptPersonDto(),
                Id = provider.Id,
                Name = provider.Name,
                CreatedDate = provider.CreatedDate,                
                DeactivateDate = provider.DeativatedDate,
                DeativateNote = provider.DeativateNote
            };
        }

        public static Provider AdaptToProvider(this ProviderDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Provider
            {
                CreatedDate = dto.CreatedDate,
                DeativatedDate = dto.DeactivateDate,
                DeativateNote = dto.DeativateNote,
                Id = dto.Id,
                Name = dto.Name,
                Person = dto.Person.AdaptPerson()
            };
        }
    }
}
