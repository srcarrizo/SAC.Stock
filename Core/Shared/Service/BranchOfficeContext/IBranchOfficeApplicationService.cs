namespace SAC.Stock.Service.BranchOfficeContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    internal interface IBranchOfficeApplicationService
    {
        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BranchOfficeDto AddBranchOffice(BranchOfficeDto branchOfficeInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BranchOfficeStaffDto AddBranchOfficeStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BranchOfficeDto ModifyBranchOffice(BranchOfficeDto branchOfficeInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BranchOfficeStaffDto ModifyFranchiseStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BranchOfficeDto> QueryBranchOffice(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        ICollection<BranchOfficeStaffDto> QueryBranchOfficeStaff(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BranchOfficeDto GetBranchOffice(Guid branchOfficeId);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        BranchOfficeStaffDto GetFranchiseStaff(Guid franchiseStaffId);
    }
}
