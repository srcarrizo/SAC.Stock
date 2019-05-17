namespace SAC.Stock.Domain.BranchOfficeContext
{
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Domain.BuyContext;
    using SAC.Stock.Domain.LocationContext;
    using SAC.Stock.Domain.SaleContext;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BranchOffice : EntityGuid
    {
        [Required(ErrorMessage = "El [Nombre] es requerido.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<BranchOfficePhone> Phones { get; set; }
        public DateTimeOffset? ActivatedDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivateNote { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Buy> Buys { get; set; }
    }
}
