namespace SAC.Stock.Service.ProfileContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    internal interface IProfileApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ProfileDto AddProfile(ProfileDto productInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ProfileDto ModifyProfile(ProfileDto profileInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ProfileDto GetProfile(int profileId);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        void RemoveProfile(int profileId);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<ProfileDto> QueryProfile(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<ProfileDto> QueryProfile(FilterInfo filterInfo = null);
    }
}
