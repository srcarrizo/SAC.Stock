namespace SAC.Stock.Service.BillContext
{
    using Seed.NLayer.Data;
    using Domain.BillContext;    
    using System.Collections.Generic;
    using System.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using System.Linq;    

    internal class BillApplicationService : IBillApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public BillDto AddBill(BillDto billDto)
        {
            var svc = new BillService(StockCtx);
            return svc.AddBill(billDto).AdaptToBillDto();
        }        

        public BillUnitTypeDto GetBillUnitType(int billUnitTypeId)
        {
            var svc = new BillService(StockCtx);
            return svc.GetBillUnitType(billUnitTypeId).AdaptToBillUnitTypeDto();
        }

        public ICollection<BillUnitTypeDto> QueryBillUnitType(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new BillService(StockCtx);
                return BillUnitTypesToDto(svc.QueryBillUnitType(pageIndex, pageSize, sortInfo, filterInfo));
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<BillDto> QueryBill()
        {
            var svc = new BillService(StockCtx);
            return svc.QueryBill().Select(b => b.AdaptToBillDto()).ToList();
        }

        private ICollection<BillUnitTypeDto> BillUnitTypesToDto(IEnumerable<BillUnitType> billUnitTypes)
        {
            var result = billUnitTypes.Select(b => b.AdaptToBillUnitTypeDto()).ToList();
            return result;
        }       
    }
}
