namespace SAC.Stock.Domain.ProfileContext
{
    using SAC.Seed.NLayer.Domain;
    public class RolesComposition : EntityAutoInc
    {
        public int ProfileId { get; set; }

        public virtual Profile Profile { get; set; }

        public string RoleCode { get; set; }

        public bool CriticalRole { get; set; }

        public int Hierarchy { get; set; }
    }
}
