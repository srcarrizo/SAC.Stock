namespace SAC.Stock.Service.ProfileContext
{
    using SAC.Seed.NLayer.Application;
    using System.Collections.Generic;
    public class ProfileDto : EntityDto<int>
    {
        public ProfileDto()
        {
            Roles = new List<RolesCompositionDto>();
        }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Hierarchy { get; set; }

        public string Scope { get; set; }

        public List<RolesCompositionDto> Roles { get; set; }
    }
}
