namespace SAC.Stock.Service.SupportContext
{
    using SAC.Stock.Service.BaseDto;
    using System.Collections.Generic;
    using System.ServiceModel;
    
    [ServiceContract]
    internal interface ISupportApplicationService
    {
        [OperationContract]
        TelcoDto GetTelco(int id);
        [OperationContract]
        TelcoDto GetTelco(string code);

        [OperationContract]
        List<TelcoDto> QueryTelco();
    }
}
