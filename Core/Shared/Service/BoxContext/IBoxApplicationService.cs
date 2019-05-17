namespace SAC.Stock.Service.BoxContext
{    
    using System.ServiceModel;    

    [ServiceContract]
    internal interface IBoxApplicationService
    {
        [OperationContract]
        BoxDto GetBox(int boxId);

        [OperationContract]
        BoxDto AddBox(BoxDto boxInfo);

        [OperationContract]
        BoxDto OpenCloseBox(BoxDto boxInfo);

        [OperationContract]
        BoxDto ModifyBox(BoxDto boxInfo);

        [OperationContract]
        BoxDto GetLatestBox();

        [OperationContract]
        BoxSalesBuysTransactionsDto GetLatestBoxWithSalesBuysTransaction();
    }
}
