namespace SAC.Stock.Service.LocationContext
{
    using System.ServiceModel;
    using Seed.NLayer.ExceptionHandling;
    using BaseDto;
    using System.Collections.Generic;
    using SAC.Seed.NLayer.Data;

    [ServiceContract]
    internal interface ILocationApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        LocationDto AddLocation(LocationDto locationInfo);
       
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        LocationDto ModifyLocation(LocationDto locationInfo);
        
        [OperationContract]
        ICollection<LocationDto> QueryChildrenLocation(int? locationId = null, bool sorted = false);
        
        [OperationContract]
        ICollection<LocationDto> QueryLocation(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);
       
        [OperationContract]
        LocationDto GetLocation(int locationId);
       
        [OperationContract]
        LocationDto GetLocation(string code);

        [OperationContract]
        LocationDto GetLocation(string name, string locTypeCode);

        [OperationContract]
        ICollection<LocationDto> QueryLocationCountry();
    }
}
