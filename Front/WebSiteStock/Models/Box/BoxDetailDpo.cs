namespace SAC.Stock.Front.Models.Box
{
    using SAC.Stock.Front.Models.Bill;
    public class BoxDetailDpo
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public BillDpo Bill { get; set; }        
    }
}