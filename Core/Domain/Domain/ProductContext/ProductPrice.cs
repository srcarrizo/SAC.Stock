namespace SAC.Stock.Domain.ProductContext
{
    using SAC.Seed.NLayer.Domain;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductPrice : EntityAutoInc
    {
        [Required(ErrorMessage = "El [Precio mayorista de compra] es requerido.")]
        public decimal BuyMayorPrice { get; set; }

        [Required(ErrorMessage = "El [Pocentaje de ganancia mayorista] es requerido.")]
        public decimal MayorGainPercent { get; set; }

        [Required(ErrorMessage = "El [Pocentaje de ganancia minorista] es requerido.")]
        public decimal MinorGainPercent { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset? DisabledDate { get; set; }
        public string DisableNote { get; set; }
        public Guid? DeactivatorUserId { get; set; }

        [Required(ErrorMessage = "El [Producto] es requerido.")]
        public int ProductId { get; set;}
        public virtual Product Product { get; set; }
    }
}