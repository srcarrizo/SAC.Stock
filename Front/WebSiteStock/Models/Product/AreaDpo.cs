namespace SAC.Stock.Front.Models.Product
{
    using System.Collections.Generic;
    public class AreaDpo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryDpo> Categories { get; set; }
    }
}