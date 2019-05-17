namespace SAC.Stock.Service.BranchOfficeContext
{
    using SAC.Membership.Service.UserManagement;
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.BranchOfficeContext;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    internal class BranchOfficeApplicationService : IBranchOfficeApplicationService
    {
        public IDataContext StockCtx { get; set; }
        public IUserManagementApplicationService UserManagementSvc { get; set; }

        public BranchOfficeDto AddBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            var svc = new BranchOfficeService(StockCtx);
            return svc.AddBranchOffice(branchOfficeInfo).AdaptToBranchOfficeDto();
        }

        public BranchOfficeStaffDto AddBranchOfficeStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo)
        {
            var svc = new BranchOfficeService(StockCtx);
            return svc.AddBranchOfficeStaff(branchOfficeStaffInfo, UserManagementSvc).AdaptToFranchiseStaffDto();
        }

        public BranchOfficeDto ModifyBranchOffice(BranchOfficeDto branchOfficeInfo)
        {
            var svc = new BranchOfficeService(StockCtx);
            return svc.ModifyBranchOffice(branchOfficeInfo).AdaptToBranchOfficeDto();
        }

        public BranchOfficeStaffDto ModifyFranchiseStaff(BranchOfficeStaffSaveDto branchOfficeStaffInfo)
        {
            var svc = new BranchOfficeService(StockCtx);
            return svc.ModifyBranchOfficeStaff(branchOfficeStaffInfo, UserManagementSvc).AdaptToFranchiseStaffDto();
        }

        public ICollection<BranchOfficeDto> QueryBranchOffice(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new BranchOfficeService(StockCtx);
                return svc.QueryBranchOffice(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToBranchOfficeDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<BranchOfficeStaffDto> QueryBranchOfficeStaff(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new BranchOfficeService(StockCtx);
                return svc.QueryBranchOfficeStaff(pageIndex, pageSize, sortInfo, filterInfo).Select(r => r.AdaptToFranchiseStaffDto()).ToList();
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public BranchOfficeDto GetBranchOffice(Guid branchOfficeId)
        {
            var svc = new BranchOfficeService(StockCtx);
            return svc.GetBranchOffice(branchOfficeId).AdaptToBranchOfficeDto();
        }

        public BranchOfficeStaffDto GetFranchiseStaff(Guid franchiseStaffId)
        {
            var svc = new BranchOfficeService(StockCtx);
            return svc.GetBranchOfficeStaff(franchiseStaffId).AdaptToFranchiseStaffDto();
        }
    }
}
