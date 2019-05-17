namespace SAC.Stock.Front.Models.Bill
{
    public class BillDpo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }        
        public int Value { get; set; }        
        public int BillUnitTypeId { get; set; } 
        public BillUnitTypeDpo BillUnitType { get; set; }
    }
}