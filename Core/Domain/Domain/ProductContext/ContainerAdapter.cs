namespace SAC.Stock.Domain.ProductContext
{
    using Service.ProductContext;
    using System.Linq;
    internal static class ContainerAdapter
    {
        public static Container AdaptToContainer(this ContainerDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Container
            {
                Id = dto.Id,
                Amount = dto.Amount,
                Description = dto.Description,
                Name = dto.Name,                
                ParentContainerId = dto.ParentContainer?.Id                
            };
        }

        public static void AdaptToContainer(this ContainerDto dto, Container to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.Id = dto.Id;
            to.Amount = dto.Amount;
            to.Description = dto.Description;
            to.Name = dto.Name;            
            to.ParentContainerId = dto.ParentContainer?.Id;                  
        }

        public static ContainerDto AdaptToContainerDto(this Container entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ContainerDto
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Description = entity.Description,
                Name = entity.Name,
                ParentContainer = entity.ParentContainer == null ? null : new ContainerDto
                {
                    Id = entity.ParentContainer.Id,
                    Amount = entity.ParentContainer.Amount,
                    Description = entity.ParentContainer.Description,
                    Name = entity.ParentContainer.Name,                                       
                },
                Containers = entity.Containers?.Select(c => c.AdaptToContainerDto()).ToList(),
                Products = entity.Products?.Select(p => p.AdaptToProductDto()).ToList()
            };
        }
    }
}
