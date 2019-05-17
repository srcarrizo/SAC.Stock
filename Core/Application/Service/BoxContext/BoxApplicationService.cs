namespace SAC.Stock.Service.BoxContext
{
    using SAC.Seed.NLayer.Data;    
    using SAC.Stock.Domain.BoxContext;        
    internal class BoxApplicationService : IBoxApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public BoxDto GetBox(int boxId)
        {
            var svc = new BoxService(StockCtx);
            return svc.GetBox(boxId).AdaptBoxToDto();
        }

        public BoxDto AddBox(BoxDto boxInfo)
        {
            var svc = new BoxService(StockCtx);
            return svc.AddBox(boxInfo).AdaptBoxToDto();
        }

        public BoxDto OpenCloseBox(BoxDto boxInfo)
        {
            var svc = new BoxService(StockCtx);
            return svc.OpenCloseBox(boxInfo).AdaptBoxToDto();
        }

        public BoxDto ModifyBox(BoxDto boxInfo)
        {
            var svc = new BoxService(StockCtx);
            return svc.ModifyBox(boxInfo).AdaptBoxToDto();
        }

        public BoxDto GetLatestBox()
        {
            var svc = new BoxService(StockCtx);
            return svc.GetLatestBox().AdaptBoxToDto();
        }

        public BoxSalesBuysTransactionsDto GetLatestBoxWithSalesBuysTransaction()
        {
            var svc = new BoxService(StockCtx);
            return svc.GetLatestBoxWithSalesBuysTransaction();
        }
    }
}
